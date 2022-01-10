import axios from "axios";
import React, { Component,  useState } from "react";
import { Button, Form, Input } from "semantic-ui-react";
import agent from "../api/agent";
interface Props
{
  saveFile : any;
  uploadFile: any;
}
function FileUpload({saveFile, uploadFile}: Props)
{
  return (
      <Form>
    <div>
    <Input className="d-none" type="file" onChange={saveFile} id="uploadID"/>
    </div>
    <div>
      
    <label onClick={uploadFile} className="btn btn-secondary" htmlFor="uploadID">Upload</label>
    </div>
      </Form>
     
  );
}

export default FileUpload;

