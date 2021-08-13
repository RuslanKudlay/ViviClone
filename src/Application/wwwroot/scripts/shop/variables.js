let currentPage = 1;
let takeItemsCount = 50;
let minPrice, maxPrice;
let oldMinPrice, oldMaxPrice;
let constMinPrice, constMaxPrice;
let searchValue;
let resultItemsCount = parseInt($('#resultCountItems').attr('totalCount'));
let allValues = {};
let brandsModel;

const keySeparator = ';';
const valueSeparator = ',';
const keyValueSeparator = '=';
const keyPriceSeparator = '-';
const noResultsLabel = "No results found";