var kitchenResponsibles = Array.from(document.getElementById("internalActivities").rows);
var btnMore = document.getElementById("seeMoreRecords");
var btnLess = document.getElementById("seeLessRecords");
var trsLength = kitchenResponsibles.length;
var defaultLength = 6;
var currentLength = 0;

doForEach(0, trsLength, hide);
doForEach(0, defaultLength, show);
checkButton();

function show(element) {
    element.style.display = '';
}

function hide(element) {
    element.style.display = 'none';
}

function doForEach(start, length, func) {
    for (i = start; i < length; i++) {
        func(kitchenResponsibles[i]);
    }
}

function moreClick() { 
    doForEach(0, trsLength, show);
    checkButton();
}

function lessClick() { 
    doForEach(defaultLength, trsLength, hide);       
    checkButton();
}

function countIfVisible(element) {
    if (element.style.display !== 'none') {
        currentLength++;
    }
}

function checkButton() {
    currentLength = 0;
    doForEach(0, trsLength, countIfVisible);
    if (currentLength === trsLength) {
        show(btnLess);
        hide(btnMore);
    } else {
        hide(btnLess);
        show(btnMore);
    }
}