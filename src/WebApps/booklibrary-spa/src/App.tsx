import {
  ChangeEvent,
  FormEvent,
  useCallback,
  useEffect,
  useState,
} from "react";
import { usePagination } from "react-use-pagination";

import { Book } from "./types/book";
import {
  HttpTransportType,
  HubConnectionBuilder,
  HubConnectionState,
} from "@microsoft/signalr";
import { Alert } from "./types/alert";
import { AlertList } from "react-bs-notifier";

function App() {
  // Keep track of length separately from data, since data fetcher depends on pagination state
  const VITE_BOOKLIBRARY_HUB_URL = "/booklibraryhub";
  const VITE_BOOKLIBRARY_API_URL = "/booklibraryapi";

  const [loading, setLoading] = useState(true);
  const [data, setData] = useState<Book[]>([]);
  const [length, setLength] = useState(0);
  const [alerts, setAlerts] = useState<Alert[]>([]);

  // Pagination hook
  const { startIndex, pageSize } = usePagination({
    totalItems: length,
    initialPageSize: 5,
  });

  const fetchUsers = useCallback(
    async ({ offset, limit }: { offset: number; limit: number }) => {
      try {
        const response = await fetch(VITE_BOOKLIBRARY_API_URL);
        if (!response.ok) {
          throw new Error("Failed to fetch data");
        }
        const json = await response.json();
        console.log(offset);
        console.log(limit);
        const result: Book[] = json;
        return result;
      } catch (error) {
        console.error("Error fetching data:", error);
      }
    },
    []
  );

  console.log(loading);

  useEffect(() => {
    const connection = new HubConnectionBuilder()
      .withUrl(VITE_BOOKLIBRARY_HUB_URL, {
        skipNegotiation: true,
        transport: HttpTransportType.WebSockets,
      })
      .build();

    connection.on(
      "BookAddedNotification",
      (message: { bookTitle: string; author: string }) => {
        const alert: Alert = {
          id: new Date().getTime(),
          type: "info",
          headline: `New Book Added!`,
          message:
            `${message.author} by ${message.author} is now available in our collection` as string,
        };
        setAlerts([alert]);
        const fetchBooks = async () => {
          try {
            const response = await fetch(VITE_BOOKLIBRARY_API_URL);
            if (!response.ok) {
              throw new Error("Failed to fetch data");
            }
            const json = await response.json();
            const result: Book[] = json as Book[];
            setData(result);
            return result;
          } catch (error) {
            console.error("Error fetching data:", error);
          }
        };
        fetchBooks();
        console.log("title");
        console.log(message.bookTitle);
        console.log("author");
        console.log(message.author);
      }
    );

    const startConnection = async () => {
      try {
        await connection.start();
        console.assert(connection.state === HubConnectionState.Connected);
        console.log("SignalR Connected.");
      } catch (err) {
        console.assert(connection.state === HubConnectionState.Disconnected);
        console.log(err);
      }
    };

    startConnection();
  }, []);

  useEffect(() => {
    const fetch = async () => {
      setLoading(true);
      const data3: Book[] = (await fetchUsers({
        offset: startIndex,
        limit: pageSize,
      })) as Book[];
      setData(data3);
      setLoading(false);
    };
    fetch();
  }, [fetchUsers, startIndex, pageSize]);

  // When data changes, update length
  useEffect(() => {
    setLength(data.length);
  }, [data]);

  const onDismissed = useCallback((alert: Alert) => {
    setAlerts((alerts) => {
      const idx = alerts.indexOf(alert);
      if (idx < 0) return alerts;
      return [...alerts.slice(0, idx), ...alerts.slice(idx + 1)];
    });
  }, []);

  const [selectedOption, setSelectedOption] = useState<string>("title");
  const [searchValue, setSearchValue] = useState<string>("");

  // Event handler to update selected option
  const handleSelectChange = (event: ChangeEvent<HTMLSelectElement>) => {
    setSelectedOption(event.target.value);
  };

  const handleSearchInputChange = (event: ChangeEvent<HTMLInputElement>) => {
    setSearchValue(event.target.value);
  };

  const handleSubmit = async (event: FormEvent<HTMLFormElement>) => {
    event.preventDefault();

    const queryParams = new URLSearchParams();
    queryParams.append(selectedOption.toLowerCase(), searchValue);

    const response = await fetch(
      `${VITE_BOOKLIBRARY_API_URL}/Search?${queryParams}`
    );
    if (response.ok) {
      const booksViewModel = await response.json();
      setData(booksViewModel as Book[]);
      console.log(booksViewModel);
    } else {
      console.error("Error:", response.statusText);
    }
  };

  return (
    <>
      <AlertList
        position="top-right"
        alerts={alerts}
        timeout={10000}
        dismissTitle="Cool!"
        onDismiss={onDismissed}
      />
      <div className="w-screen min-h-screen bg-booklibrary bg-cover bg-fixed bg-bottom flex flex-col justify-center align-middle py-7">
        <div className="bg-black max-w-xl w-full self-center py-6">
          <h1 className="font-brand text-white text-4xl text-center">
            Royal Library
          </h1>
        </div>
        <div className="bg-white max-w-3xl w-full flex justify-center align-middle self-center mt-8 rounded-lg py-6">
          <form
            className="flex justify-center align-middle"
            onSubmit={handleSubmit}
          >
            <div className="my-auto mr-3">
              <p>Search By</p>
            </div>
            <div className="mr-3">
              <select
                className="form-select rounded-lg border-slate-500 focus:border-black active:border-black"
                onChange={handleSelectChange}
                value={selectedOption}
              >
                <option value="Title">Title</option>
                <option value="Category">Category</option>
                <option value="Type">Type</option>
              </select>
            </div>
            <div className="mr-3">
              <input
                type="text"
                placeholder="Enter search value..."
                className="form-input rounded-lg border-slate-500 focus:border-black active:border-black"
                onChange={handleSearchInputChange}
                value={searchValue}
              />
            </div>
            <div>
              <button
                type="submit"
                className="bg-gray-900 text-white rounded-lg px-6 py-2"
              >
                Search
              </button>
            </div>
          </form>
        </div>
        <div className="bg-white max-w-5xl w-full flex justify-center align-middle self-center mt-8 rounded-lg py-6">
          <table className="table-auto border text-sm">
            <thead>
              <tr>
                <th className="border py-2 px-2 min-w-64">Book Title</th>
                <th className="border py-2 px-2 min-w-32">Publisher</th>
                <th className="border py-2 px-2 min-w-32">Authors</th>
                <th className="border py-2 px-2 min-w-28">Type</th>
                <th className="border py-2 px-2">ISBN</th>
                <th className="border py-2 px-2 min-w-28">Category</th>
                <th className="border py-2 px-2">Available Copies</th>
              </tr>
            </thead>
            <tbody>
              {data.map((book: Book) => {
                return (
                  <tr>
                    <td className="border py-2 px-2">{book.title}</td>
                    <td className="border py-2 px-2">{book.publisher}</td>
                    <td className="border py-2 px-2">{book.author}</td>
                    <td className="border py-2 px-2 text-center">
                      {book.type}
                    </td>
                    <td className="border py-2 px-2 text-center">
                      {book.isbn}
                    </td>
                    <td className="border py-2 px-2 text-center">
                      {book.category}
                    </td>
                    <td className="border py-1 px-1 text-center">
                      {book.availableCopies}
                    </td>
                  </tr>
                );
              })}
            </tbody>
          </table>
        </div>
      </div>
    </>
  );
}

export default App;
