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
    <Input type="file" onChange={saveFile} />
    </div>
    <div>
      <Button onClick={uploadFile}>Upload</Button>
    </div>
      </Form>
     
  );
}

export default FileUpload;

