{
    const searchBtn = document.getElementById("search_button");
    const searchInput = document.getElementById("search_input");
    searchBtn.addEventListener("click", () => {
        currentUrl = window.location.href;
        urlObject = new URL(currentUrl);
        searchParams = urlObject.searchParams;
        if (searchParams.has("search")) searchParams.set("search", searchInput.value);
        else searchParams.append("search", searchInput.value);
        updatedUrl = urlObject.origin + urlObject.pathname + "?" + searchParams.toString();
        window.location.href = updatedUrl;
    });
}