import React, { useState, useEffect } from "react";


// const data
const preUrl = "http://localhost:5000/api"

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

// function for display json from request

const url = `${preUrl}/UploadFiles`;
const files = JSON.parse(sendGET(url));
//console.log(files);


export default function TableFileStatus ({curSel,tdClickCB}) {
    const DisplayData = files.map(
        (info) => {
            return (
                <tr key={info.id}  data-id={info.id}>
                    <td key={info.id}>{Date.now.toString("dd.MM.yyyy")}</td>
                    <td key={info.id} data-id={info.id} className={curSel == info.id ? 'active' : ''} onClick={tdClickCB}>{info.name} | {info.id}</td>
                    <td key={info.DocumentType}>{info.DocumentType}</td>
                    <td key={info.IssueReason}>{info.IssueReason}</td>
                    <td key={info.Language}>{info.Language}</td>
                    <td key={info.DocumentStatus.Status}>{info.DocumentStatus.Status}</td>
                </tr>
            )
        }
    )
    
    return (
            <div className="container">
                <p>Первая таблица</p>
                <table>
                <thead>
                    <tr>
                        <th>Дата/Время</th>
                        <th>Номер документа</th>
                        <th>Тип документа</th>
                        <th>Причина выпуска</th>
                        <th>Язык</th>
                        <th>Статус</th>
                    </tr>
                </thead>
                    <tbody>
                    {DisplayData}
                    </tbody>
                </table>
        </div>
    )
}