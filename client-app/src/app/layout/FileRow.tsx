import React, { useState } from 'react'
import { Header, Table, Rating, Icon, Button } from 'semantic-ui-react'
import {FileModel} from '../models/FileModel';

interface Props{
  fileModel: FileModel;
  selectDocument: (id: number)=>void;
  };

//const [stats, setStats] = useState(false);
// const state  = {
//   active: false,
// };
// const handleClick=()=>{
// this.setState({active: true});
// };

const FileRow = ({fileModel, selectDocument} : Props) => (
        <Table.Row>
        <Table.Cell textAlign='center' singleLine>
          {/* <Button onClick={()=>{setStats(true)}} style={{backgroundColor:stats==true?"blue":"red"}} content={fileModel.name}/>  */}
          <Button content={fileModel.name}/> 
        </Table.Cell>
        <Table.Cell textAlign='center' singleLine>{fileModel.type}</Table.Cell>
        <Table.Cell textAlign='center' singleLine>{fileModel.pageNumber}</Table.Cell>
        <Table.Cell textAlign='center' singleLine>          
            <Icon color='green' name='checkmark' size='large' />          
          </Table.Cell>
        <Table.Cell textAlign='center' singleLine>
        <Icon color='green' name='checkmark' size='large' />
        </Table.Cell>
      </Table.Row>
)

export default FileRow;