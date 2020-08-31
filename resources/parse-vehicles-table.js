var tableAttr = '[data-bind="foreach: vehicleRefModel.vehicleReferences"]';

var data = [];
var tableRows = document.querySelectorAll(tableAttr)[0].querySelectorAll("tr");

for(var i = 0; i<tableRows.length; i++)
{
    if(i == 0)
        continue;

        var item = {};
    var row = tableRows[i];
    var cells = row.querySelectorAll("td");
    item.type = cells[0].textContent;
    item.weight = cells[1].textContent;
    item.hull = cells[2].textContent;
    item.handling = parseInt(cells[3].textContent);
    item.maxGear = parseInt(cells[4].textContent);
    item.slots = parseInt(cells[5].textContent);
    item.crew = parseInt(cells[6].textContent);
    item.keywords = cells[7].textContent;
    item.cost = parseInt(cells[8].textContent);

    data.push(item);
}

console.log(data);