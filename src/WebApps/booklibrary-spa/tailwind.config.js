/** @type {import('tailwindcss').Config} */
export default {
  content: ["./index.html", "./src/**/*.{js,ts,jsx,tsx}"],
  theme: {
    extend: {
      backgroundImage: {
        booklibrary:
          "url('https://res.cloudinary.com/df50ukmul/image/upload/f_auto,q_auto/3d-rendering-classic-interior_my7ios')",
      },
      fontFamily: {
        brand: ["Berkshire Swash", "sans-serif"],
        sans: ["Schibsted Grotesk", "sans-serif"],
      },
    },
  },
  plugins: [require("@tailwindcss/forms")],
};
