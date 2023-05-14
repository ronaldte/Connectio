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

