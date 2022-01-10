// const data
const preUrl = "http://localhost:5000/api/"

// function for send request
function sendGET(query) {
    let allText;
    let xhr = new XMLHttpRequest();
    xhr.open("GET", query, false);

    xhr.onreadystatechange = function () {
        if (xhr.readyState === 4) {
            if (xhr.status === 200 || xhr.status == 0) {
                allText = xhr.responseText;
            } else allText = "";
        }
    }
    xhr.send(null);
    return allText;
}

//console.log(files);

export default function TableDocument({dataId}) {

      const url = preUrl + "UploadFiles/" + dataId;
      const files = JSON.parse(sendGET(url));
      console.log(files);

      console.log(files.length)
      const tablesList = () => {
            return (
                <tr key={files.Id}>
                    <td key={files.DocumentNumber}>{files.DocumentNumber}</td>
                    <td key={files.Id}>{files.Id}</td>
                    <td key={files.Id}>{files.Id}</td>
                    <td key={files.Id}>{files.Id}</td>
                    <td key={files.Id}>{files.Id}</td>
                </tr>
            )
        }


    return(
        <div className="container">
          <p>Вторая таблица</p>
          <table>
          <thead>
            <tr>
              <th>Дата/Время</th>
              <th>Номер документа</th>
              <th>Тип документа</th>
              <th>Язык</th>
              <th>Ревизия</th>
              <th>Лист</th>
            </tr>
          </thead>
            <tbody>               
                <tr key={files.Id}>
                    <td key={files.DateCreated}>{files.DateCreated}</td>
                    <td key={files.Id}>{files.DocumentNumber}</td>
                    <td key={files.Id}>{files.DocumentType}</td>
                    <td key={files.Id}>{files.Language}</td>
                    <td key={files.Id}>{files.Revision}</td>
                    <td key={files.Id}>{files.Sheets.length}</td>
                </tr>
            </tbody>
          </table>
        </div>
    );
}