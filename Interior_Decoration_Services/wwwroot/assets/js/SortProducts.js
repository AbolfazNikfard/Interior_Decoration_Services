{
    let currentUrl, urlObject, searchParams, selectedSortOptionValue, updatedUrl, paramName = "sort";
    const selectSortOptionElement = document.getElementById("SortProduct");
    selectedSortOptionValue = selectSortOptionElement.value;
    currentUrl = window.location.href;
    urlObject = new URL(currentUrl);
    searchParams = urlObject.searchParams;
    if (searchParams.has(paramName)) {
        let paramValue = searchParams.get(paramName);
        let optionElement;
        switch (paramValue) {
            case "Newest":
                optionElement = getOptionByValue("Newest", selectSortOptionElement);
                break;
            case "Oldest":
                optionElement = getOptionByValue("Oldest", selectSortOptionElement);
                break;
            case "ExpensiveToCheap":
                optionElement = getOptionByValue("ExpensiveToCheap", selectSortOptionElement);
                break;
            case "CheapToExpensive":
                optionElement = getOptionByValue("CheapToExpensive", selectSortOptionElement);
                break;
            case "AlphabetAscending":
                optionElement = getOptionByValue("AlphabetAscending", selectSortOptionElement);
                break;
            case "AlphabetDescending":
                optionElement = getOptionByValue("AlphabetDescending", selectSortOptionElement);
                break;
            default:
                optionElement = getOptionByValue("null");
        }
        selectSortOptionElement.children[0].removeAttribute("selected");
        optionElement.setAttribute("selected", "");
    }
    selectSortOptionElement.addEventListener("change", function () {
        selectedSortOptionValue = selectSortOptionElement.value;
        currentUrl = window.location.href;
        urlObject = new URL(currentUrl);
        searchParams = urlObject.searchParams;

        if (searchParams.has(paramName)) searchParams.set(paramName, selectedSortOptionValue);
        else searchParams.append(paramName, selectedSortOptionValue);

        updatedUrl = urlObject.origin + urlObject.pathname + "?" + searchParams.toString();
        window.location.href = updatedUrl;
    });
    function getOptionByValue(value,selectElement) {
        for (const option of selectElement.options) {
            if (option.value === value) {
                return option;
            }
        }
    }
}