﻿let rowCounter = 0;

function DocumentAddItem() {
    const itemId = "item" + rowCounter++;

    //items
    const removeButton = document.createElement("button");
    removeButton.innerHTML = "-";
    removeButton.className = "btn btn-outline-danger";
    removeButton.type = "button";
    removeButton.id = itemId;
    removeButton.onclick = function () {
        RemoveItemId(removeButton.id);
    };

    const ItemNameInput = document.createElement("input");
    ItemNameInput.name = "ItemName";
    ItemNameInput.className = "form-control";

    const ItemPriceInput = document.createElement("input");
    ItemPriceInput.id = itemId + "_price";
    ItemPriceInput.name = "ItemPrice";
    ItemPriceInput.className = "form-control";
    ItemPriceInput.type = "number";
    ItemPriceInput.min = 0;
    ItemPriceInput.onchange = function () {
        var rowItem = document.getElementById(itemId);
        const price = rowItem.childNodes[2].childNodes[0].value;
        const amount = rowItem.childNodes[3].childNodes[0].value;
        rowItem.childNodes[5].childNodes[0].value = price * amount;
    }

    const ItemAmountInput = document.createElement("input");
    ItemAmountInput.id = itemId + "_amount";
    ItemAmountInput.name = "ItemAmount";
    ItemAmountInput.className = "form-control";
    ItemAmountInput.type = "number";
    ItemAmountInput.value = 1;
    ItemAmountInput.min = 1;
    ItemAmountInput.onchange = function () {
        var rowItem = document.getElementById(itemId);
        const price = rowItem.childNodes[2].childNodes[0].value;
        const amount = rowItem.childNodes[3].childNodes[0].value;
        rowItem.childNodes[5].childNodes[0].value = price * amount;
    }

    const ItemUnitInput = document.createElement("input");
    ItemUnitInput.name = "ItemUnit";
    ItemUnitInput.className = "form-control";

    const ItemTotalAmount = document.createElement("input");
    ItemTotalAmount.id = itemId + "_total";
    ItemTotalAmount.id = rowCounter;
    ItemTotalAmount.className = "form-control";
    ItemTotalAmount.disabled = true;

    //cols
    const removeButtonCol = document.createElement("div");
    removeButtonCol.className = "col-md-1";
    removeButtonCol.appendChild(removeButton);

    const ItemNameInputCol = document.createElement("div");
    ItemNameInputCol.className = "col-md-5";
    ItemNameInputCol.appendChild(ItemNameInput);

    const ItemPriceInputCol = document.createElement("div");
    ItemPriceInputCol.className = "col";
    ItemPriceInputCol.appendChild(ItemPriceInput);

    const ItemAmountInputCol = document.createElement("div");
    ItemAmountInputCol.className = "col";
    ItemAmountInputCol.appendChild(ItemAmountInput);

    const ItemUnitInputCol = document.createElement("div");
    ItemUnitInputCol.className = "col";
    ItemUnitInputCol.appendChild(ItemUnitInput);

    const ItemTotalAmountCol = document.createElement("div");
    ItemTotalAmountCol.className = "col";
    ItemTotalAmountCol.appendChild(ItemTotalAmount);


    //row
    const ItemRow = document.createElement("div");
    ItemRow.className = "row";
    ItemRow.appendChild(removeButtonCol);
    ItemRow.appendChild(ItemNameInputCol);
    ItemRow.appendChild(ItemPriceInputCol);
    ItemRow.appendChild(ItemAmountInputCol);
    ItemRow.appendChild(ItemUnitInputCol);
    ItemRow.appendChild(ItemTotalAmountCol);
    ItemRow.id = itemId;

    //parent
    const parent = document.getElementById("DocumentItemsBox");
    parent.appendChild(ItemRow);
}

function RemoveItemId(itemId) {
    var rowItem = document.getElementById(itemId);
    rowItem.remove();
}

$(document).ready(function () {
    DocumentAddItem();
});
