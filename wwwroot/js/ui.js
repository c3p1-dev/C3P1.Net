function togglemode() {
    var currentTheme = getmode();

    if (currentTheme == 'dark') {
        document.documentElement.setAttribute('data-bs-theme', 'light');
    }
    else if (currentTheme == 'light') {
        document.documentElement.setAttribute('data-bs-theme', 'dark');
    }
}

function darkmode() {
    document.documentElement.setAttribute('data-bs-theme', 'dark');
}
function lightmode() {
    document.documentElement.setAttribute('data-bs-theme', 'light');
}

function getmode() {
    return document.documentElement.getAttribute('data-bs-theme');
}