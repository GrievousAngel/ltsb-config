function addProperty() {
    let propertyName = prompt("New property name");
    if (propertyName != null) {
        let btnGroup = document.getElementById("btn-group");

        let rowDiv = document.createElement('div');
        rowDiv.classList.add("row", "mb-3");
        rowDiv.innerHTML = `
            <label class="col-sm-2 col-form-label">${propertyName}</label>
            <div class="col-sm-10">
                <input class="form-control" type="text" id="Data_Properties_${propertyName}_" name="Data.Properties[${propertyName}]" value="">
            </div>`;

        btnGroup.parentNode.insertBefore(rowDiv, btnGroup);
    }

}

document.getElementById("btn-new").addEventListener("click", addProperty);