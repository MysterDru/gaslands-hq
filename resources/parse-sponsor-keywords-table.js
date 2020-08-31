var data = [];
var tableRows = document.querySelectorAll(".referenceTable")[1].querySelectorAll("tr");

for(var i = 0; i<tableRows.length; i++)
{
    if(i == 0)
        continue;

    var item = {};
    var row = tableRows[i];
    var cells = row.querySelectorAll("td");
    item.keyword = cells[0].textContent;
    item.description = cells[1].textContent;
    item.type= "Sponsor";

    data.push(item);
}

console.log(JSON.stringify(data));