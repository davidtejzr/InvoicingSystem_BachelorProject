let rowCounter = 0;

function DocumentAddItem() {
    document.getElementById("DocumentWarningMessageDiv").style = "display: none !important;";
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
    ItemNameInput.required = true;

    const ItemPriceInput = document.createElement("input");
    ItemPriceInput.id = itemId + "_price";
    ItemPriceInput.name = "ItemPrice";
    ItemPriceInput.className = "form-control";
    ItemPriceInput.type = "number";
    ItemPriceInput.min = 0;
    ItemPriceInput.value = "0.00"
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
    ItemTotalAmount.value = "0.00";

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


function DocumentAddItemWithValues(name, price, amount, unit) {
    document.getElementById("DocumentWarningMessageDiv").style = "display: none !important;";
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
    ItemNameInput.required = true;
    ItemNameInput.value = name;

    const ItemPriceInput = document.createElement("input");
    ItemPriceInput.id = itemId + "_price";
    ItemPriceInput.name = "ItemPrice";
    ItemPriceInput.className = "form-control";
    ItemPriceInput.type = "number";
    ItemPriceInput.min = 0;
    ItemPriceInput.value = price;
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
    ItemAmountInput.value = amount;
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
    ItemUnitInput.value = unit;

    const ItemTotalAmount = document.createElement("input");
    ItemTotalAmount.id = itemId + "_total";
    ItemTotalAmount.id = rowCounter;
    ItemTotalAmount.className = "form-control";
    ItemTotalAmount.disabled = true;
    ItemTotalAmount.value = "0.00";

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
    if (rowCounter > 1) {
        var rowItem = document.getElementById(itemId);
        rowItem.remove();
        rowCounter--;
    }
    else {
        document.getElementById("DocumentWarningMessageDiv").style = "display: relative";
        document.getElementById("DocumentWarningMessage").innerHTML = "Doklad musí mít alespoň jednu položku!";
    }
}


function CustomerSelect() {
    var customerName = document.getElementById("customerSelector").value;

    $.ajax({
        type: "GET",
        url: "/Document/CustomerData",
        dataType: "json",
        data: {
            name: customerName
        }

    }).done(function (data) {


        document.getElementById("labelCustomerAddressFirstRow").innerHTML = "prvni radek";
        document.getElementById("labelCustomerAddressSecondRow").innerHTML = "druhy radej";
        document.getElementById("labelCustomerIco").innerHTML = "ico";
        document.getElementById("labelCustomerDic").innerHTML = "dic";

    });
}