
function NavBarStatus() {
    let element = document.getElementById("nav");
    element.style.background = window.scrollY > 10 ? "black" : "transparent";
}