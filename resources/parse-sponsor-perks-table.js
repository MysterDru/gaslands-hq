var data = [];
var tableRows = document.querySelectorAll(".referenceTable")[7].querySelectorAll("tr");

var currentPerk = "";

for(var i = 0; i<tableRows.length; i++)
{
    if(i == 0)
        continue;

    var item = {};
    var row = tableRows[i];
    var cells = row.querySelectorAll("td");

    if(cells.length == 1) {
        console.log(JSON.stringify(data));

        data = [];

        currentPerk = cells[0].textContent;
        continue;
    }

    item.category = currentPerk;
    item.name = cells[0].textContent;
    item.description = cells[1].textContent;
    item.cost = cells[2].textContent;

    data.push(item);
}

// console.log(JSON.stringify(data));