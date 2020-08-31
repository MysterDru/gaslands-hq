var data = [];
var tableRows = document.querySelectorAll("#weaponsTabPage .referenceTable")[0].querySelectorAll("tr");

for(var i = 0; i<tableRows.length; i++)
{
    if(i == 0)
        continue;

    var item = {};
    var row = tableRows[i];
    var cells = row.querySelectorAll("td");
    item.name = cells[0].textContent;
    item.type = cells[1].textContent;
    item.attack= cells[2].textContent;
    item.range = cells[3].textContent;
    item.ammo = cells[4].textContent === "-" ? 0 : parseInt(cells[4].textContent);
    item.slots= cells[5].textContent === "-" ? 0 : parseInt(cells[5].textContent);
    item.crewFired = cells[6].textContent === "Yes";
    item.specialRules= cells[7].textContent;
    item.cost= cells[8].textContent === "-" ? 0 : parseInt(cells[8].textContent);

    data.push(item);
}

console.log(JSON.stringify(data));