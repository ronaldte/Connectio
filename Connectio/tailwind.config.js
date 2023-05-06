/** @type {import('tailwindcss').Config} */
module.exports = {
    content: ["./**/*.{razor,cshtml,html}"],
    mode: 'jit',
    theme: {
        extend: {},
    },
    plugins: [],
    safelist: [
        'font-bold',
        'text-orange-400',
        'border-orange-400',
        'border-transparent',
        'hover:text-gray-600',
        'hover:border-gray-300'
    ]
}

