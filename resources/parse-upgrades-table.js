var data = [];
var tableRows = document.querySelectorAll("#upgradesTabPage .referenceTable")[0].querySelectorAll("tr");

for(var i = 0; i<tableRows.length; i++)
{
    if(i == 0)
        continue;

    var item = {};
    var row = tableRows[i];
    var cells = row.querySelectorAll("td");
    item.upgrade = cells[0].textContent;
    item.slots = cells[1].textContent;
    item.ammo = cells[2].textContent;
    item.rules = cells[3].textContent;
    item.cost = cells[4].textContent;

    data.push(item);
}

console.log(JSON.stringify(data));