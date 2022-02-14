import React, { useEffect, useState } from 'react';
import { Button, Container, Form, Grid, Input, Table } from 'semantic-ui-react';
import NavBar from '../layout/NavBar';
import { v4 as uuid } from 'uuid';
import agent from '../api/agent';
import { FileModel } from '../models/FileModel';
import FileRow from './FileRow';
import FileUploadComponent from './FileUploadComponent';
import { DocumentAttributesModel } from '../models/DocumentAttributesModel';
import Header from '../../components/Header';
import { AnyRecord } from 'dns';
import SVGWatcher from '../../components/Watcher';
import AttributesTable from './AttributesTable';
import { AttributesModel } from '../models/AttributesModel';
import { Console } from 'console';
import Loader from '../../components/Loader';

function App() {
    
  const [file, setFile] = useState<any>();
  const [fileName, setFileName] = useState("");

  const saveFile = (e: any) => {

    setFile(e.target.files[0]);
    setFileName(e.target.files[0].name);
  };

  const uploadFile = (e: any) => {
    const formData = new FormData();
    formData.append("file", file);
    formData.append("fileName", fileName);

    try {
      const res = agent.Files.upload(formData).then();
      console.log(res);
    }
    catch (ex) {
      console.log(ex);
    };  
   
  };

  const [selectedFile, setSelectedFile] = useState(null);

  const [files, setFiles] = useState<FileModel[]>([]);
  useEffect(() => {
    agent.Files.list().then(response => {
      setFiles(response);
    })
  }, []);


  const [selectedDocumentAttributes, setselectedDocumentAttributes] = useState<AttributesModel | undefined>(undefined);

  const [selectedDocumentImage, setselectedDocumentImage] = useState<string>("");

  // const [DocumentAttributes, setDocumentAttributes] = useState<AttributesModel>();
  // useEffect(() => {
  //   agent.Attributes.element(id).then(responce => {setDocumentAttributes = responce;})
  // },[]);

  function handleSelectDocumentAttributes(id: number) {

    agent.Attributes.element(id).then(responce=>
    {
      setselectedDocumentAttributes(responce); 
      if(responce.FileName == null)
      {
        console.log("no attributes");
        agent.Attributes.saveElement(id).then();
      }
    })

    agent.Image.image(id).then(res=>
      {
        setselectedDocumentImage(res);
        if(res == null)
        {
            console.log("no image");
        }
      })
  }; 

  return (
    <>
      <Header />
      <Grid>
        <div className="file-uploader" style={{ marginTop: '20px', marginLeft: '70px' }}>
          <FileUploadComponent
            saveFile={saveFile}
            uploadFile={uploadFile} />

        </div>
      </Grid>
      <Grid>
        <div style={{ marginTop: '5px', height: '250px', width: '650px', marginLeft: '70px', overflowY: 'scroll', border: '10px' }}>
          <Table >
            <Table.Header>
              <Table.Row textAlign='center'>
                <Table.HeaderCell >Документ</Table.HeaderCell>
                <Table.HeaderCell>Тип</Table.HeaderCell>
                <Table.HeaderCell>Номер листа</Table.HeaderCell>
                <Table.HeaderCell>Статус загрузки</Table.HeaderCell>
              </Table.Row>
            </Table.Header>
            <Table.Body>
              {files.map((file: FileModel) => (
                <FileRow
                  file={file}
                  selectDocument={handleSelectDocumentAttributes}
                />
              ))}
            </Table.Body>
          </Table>
        </div>
      </Grid>
      <Grid>
        {selectedDocumentAttributes !== undefined && 
        <div style={{ marginTop: '10px', height: '300px', width: '650px', marginLeft: '70px', overflowY: 'scroll', border: '10px' }}>
          {/* <Loader/> */}
          <AttributesTable
            selectedDocumentAttributes={selectedDocumentAttributes}
          />
        </div> } 
        {selectedDocumentAttributes !== undefined && 
        <div style={{ marginTop: '-250px', marginLeft: '20px', border: '10px' }}>
        {/* <Loader/> */}
        <SVGWatcher
        image={selectedDocumentImage}/>
        </div> 
}        
      </Grid>
      <Grid>
      
      </Grid>
    </>
  );
}

export default App;


  function usePromiseTracker(): { promiseInProgress: any; } {
    throw new Error('Function not implemented.');
  }
// function FIleModel(fileId: any, FIleModel: any) {
//   throw new Error('Function not implemented.');
// }

