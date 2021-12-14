import React, { useEffect, useState } from 'react';
import { Button, Container, Form, Grid, Input, Table} from 'semantic-ui-react';
import { Activity } from '../models/activity';
import NavBar from '../layout/NavBar';
import ActivityDashboard from '../../features/activities/dashboard/ActivityDashboard';
import {v4 as uuid} from 'uuid';
import agent from '../api/agent';
import LoadingComponent from '../layout/LoadingComponent';
import { FileModel } from '../models/FileModel';
import FileRow from './FileRow';
import AttributesList from './AttributesTable';
import { AttributesModel } from '../models/AttributesModel';
import AttributeCard from './AttributesTable';
import FileUploadComponent from './FileUploadComponent';
import { unstable_renderSubtreeIntoContainer } from 'react-dom';
import { DocumentAttributesModel } from '../models/DocumentAttributesModel';
import DocumentAttributesList from './DocumentAttributesList';
import ListTemplate from './ListTemplate';

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
      <NavBar />
      <div className="file-uploader" style={{marginTop: '60px', marginLeft:'40px'}}>
      <FileUploadComponent 
      saveFile={saveFile}
      uploadFile={uploadFile}
      />
    </div>
      <Grid>
      <div style={{ border: '10px', justifyContent:'center', alignItems:'center', overflowY: 'scroll', height: '50vh', width: '100vh', marginTop: '30px', marginLeft:'40px'}}>
     <Table >
        <Table.Header>
        <Table.Row textAlign='center'>
        <Table.HeaderCell >Document</Table.HeaderCell>
        <Table.HeaderCell>Type</Table.HeaderCell>
        <Table.HeaderCell>Page number</Table.HeaderCell>
        <Table.HeaderCell>Upload status</Table.HeaderCell>
        <Table.HeaderCell>Check status</Table.HeaderCell>
        </Table.Row>
      </Table.Header>
      <Table.Body>
        {files.map((file: FileModel) => (
            <FileRow 
            fileModel={file}
            selectDocument={handleSelectDocumentAttributes}
            />
            ))}
      </Table.Body>    
      </Table>     
    </div>  
    <div style={{marginTop: '30px',height: '400px',  width: '650px', marginLeft: '70px',  overflowY: 'scroll', border: '10px'}}>
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

