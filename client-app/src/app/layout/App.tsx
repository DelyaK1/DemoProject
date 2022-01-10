import React, { useEffect, useState } from 'react';
import { Button, Container, Form, Grid, Input, Table} from 'semantic-ui-react';
import NavBar from '../layout/NavBar';
import {v4 as uuid} from 'uuid';
import agent from '../api/agent';
import { FileModel } from '../models/FileModel';
import FileRow from './FileRow';
import FileUploadComponent from './FileUploadComponent';
import { DocumentAttributesModel } from '../models/DocumentAttributesModel';
import DocumentAttributesList from './DocumentAttributesList';
import Header from '../../components/Header';




function App() {

  const [file, setFile] = useState<any>();
  const [fileName, setFileName] = useState("");

  const saveFile = (e: any) => {
    setFile(e.target.files[0]);
    setFileName(e.target.files[0].name);
  };

  const uploadFile = async (e: any) => {
    const formData = new FormData();
    formData.append("file", file);
    formData.append("fileName", fileName);
    try {
      const res = agent.Files.upload(formData).then();
      console.log(res);
    } catch (ex) {
      console.log(ex);
    };
    try{
    const res  = agent.Attributes.save().then();
    console.log(res);

    }
    catch(ex){
      console.log(ex);

    }
  }; 

const [selectedFile, setSelectedFile] = useState(null);

const [files, setFiles] = useState<FileModel[]>([]);
useEffect(() => {
  agent.Files.list().then(response =>
  {
    setFiles(response);
  })
}, []);


const[selectedDocumentAttributes, setselectedDocumentAttributes]=useState<DocumentAttributesModel | undefined>(undefined);

const [DocumentAttributes, setDocumentAttributes] = useState<DocumentAttributesModel[]>([]);
useEffect(() => {
  agent.Attributes.list().then(response =>
  {
    setDocumentAttributes(response);
  })
}, []);

function handleSelectDocumentAttributes(id: number)
{
  setselectedDocumentAttributes(DocumentAttributes.find(x=>x.docId===id))
};

  return (
    <>
      <Header/>
      <Grid>
      <div className="file-uploader" style={{marginTop: '20px', marginLeft:'70px'}}>
      <FileUploadComponent 
      saveFile={saveFile}
      uploadFile={uploadFile}/>

      </div>
      </Grid>
      <Grid>
      <div style={{marginTop: '5px',height: '120px',  width: '650px', marginLeft: '70px',  overflowY: 'scroll', border: '10px'}}>
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
    <div style={{marginTop: '5px',height: '400px',  width: '650px', marginLeft: '70px',  overflowY: 'scroll', border: '10px'}}>
        <DocumentAttributesList
        selectedDocumentAttributes={DocumentAttributes[0]} 
        />  
    </div> 
    </Grid>              
    </>
  );
}

export default App;

// function FIleModel(fileId: any, FIleModel: any) {
//   throw new Error('Function not implemented.');
// }

