const path = '/Post/Index/';
const actuall_pathname = window.location.pathname;

if (!actuall_pathname.startsWith(path)) {
    const posts = document.querySelectorAll('.connectio-post');

    posts.forEach(post => {
        post.addEventListener('click', function (event) {
            window.location.assign(path + post.id)
        })
        post.classList.add("hover:cursor-pointer", "hover:bg-slate-100","transition","ease","duration-500");
    })
}

window.addEventListener("DOMContentLoaded", (event) => {
    const button = document.getElementById("search-button");
    const input_field = document.getElementById("search-input");
    if (input_field.value.length == 0) {
        button.disabled = true;
        button.classList.add("cursor-not-allowed");
    }
});

document.getElementById("search-input").addEventListener('keyup', (event) => {
    const button = document.getElementById("search-button");
    if (event.target.value.length != 0) {
        button.disabled = false;
        button.classList.remove("cursor-not-allowed");
    } else {
        button.disabled = true;
        button.classList.add("cursor-not-allowed");        
    }
})

function changeTab(event, tab) {
    document.querySelectorAll(".tab-link").forEach(tab => {
        tab.classList.remove("border-orange-400", "text-orange-400");
        tab.classList.add("hover:text-orange-400", "hover:border-orange-400", "hover:cursor-pointer");
    })

    event.target.classList.add("border-orange-400", "text-orange-400");

    document.querySelectorAll(".search-content").forEach(contentTab => {
        contentTab.hidden = true;
    })

    document.getElementById(tab).removeAttribute("hidden");
}
