var data = [];
var tableRows = document.querySelectorAll("#sponsorsTabPage .referenceTable")[0].querySelectorAll("tr");

for(var i = 0; i<tableRows.length; i++)
{
    if(i == 0)
        continue;

    var item = {};
    var row = tableRows[i];
    var cells = row.querySelectorAll("td");
    item.name = cells[0].textContent;
    item.perkClasses = cells[1].textContent.split(", ");
    item.keywords = cells[2].textContent;

    data.push(item);
}

console.log(JSON.stringify(data));