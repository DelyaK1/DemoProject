import axios from "axios";
import React, { Component, useState } from "react";
import { Button, Form, Input } from "semantic-ui-react";
import agent from "../api/agent";
interface Props {
  saveFile: any;
  uploadFile: any;
}
function FileUpload({ saveFile, uploadFile }: Props) {
  return (
    <Form className="upload-form">
      <div>
        <Input className="d-none" type="file" onChange={saveFile} id="uploadID" />
      </div>
      <div>

        <label className="btn btn-secondary" htmlFor="uploadID"style={{backgroundColor:'#CACFD2', borderColor:'#CACFD2'}}>выбрать файл</label>
        <a> </a>
        <button className="btn btn-primary" onClick={uploadFile}>загрузить</button>
      </div>
    </Form>

  );
}

export default FileUpload;
