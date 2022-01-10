import React, { useState } from 'react'
import { Header, Table, Rating, Icon, Button } from 'semantic-ui-react'
import {FileModel} from '../models/FileModel';

interface Props{
  file: FileModel;
  selectDocument: (id: number)=>void;
  };
const FileRow = ({file, selectDocument} : Props) => (

        <Table.Row>
        <Table.Cell textAlign='center' singleLine>
        <a href='#' style={{color: "black"}}>{file.name.split('.pdf')[0]}</a>
        <a href="">{selectDocument}</a>
        </Table.Cell>         
        <Table.Cell textAlign='center' singleLine>{file.type}</Table.Cell>
        <Table.Cell textAlign='center' singleLine>{file.pageNumber}</Table.Cell>
        <Table.Cell textAlign='center' singleLine>          
            <Icon color='green' name='checkmark' size='large' />          
          </Table.Cell>
      </Table.Row>
)

export default FileRow;