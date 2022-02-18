import { stringify } from 'querystring';
import React from 'react';
import {Table } from 'semantic-ui-react';
import { AttributesModel } from '../models/AttributesModel';
import Loader from './Loader';

interface Props
{
  isLoading: boolean;
  selectedDocumentAttributes: AttributesModel|undefined;
}
  export default function AttributesTable({selectedDocumentAttributes, isLoading}: Props)
{
  return isLoading ? (<Loader />) : 
    (
    <Table>
    <Table.Header>
      <Table.Row>
        <Table.HeaderCell>Атрибут</Table.HeaderCell>
        <Table.HeaderCell>Значение</Table.HeaderCell>
      </Table.Row>
    </Table.Header>

    <Table.Body>
    <Table.Row>
      <Table.Cell>Page:</Table.Cell>
      <Table.Cell >
        <div style={{color: selectedDocumentAttributes?.Page == -1 ? 'red':""}}>{selectedDocumentAttributes?.Page}</div>
      </Table.Cell>
    </Table.Row>
    <Table.Row>
      <Table.Cell>File name:</Table.Cell>
      <Table.Cell>
       <div style={{color: selectedDocumentAttributes?.FileName == 'N/D' || selectedDocumentAttributes?.FileName == 'warning' ? 'red':""}}>{selectedDocumentAttributes?.FileName}</div>
      </Table.Cell>
    </Table.Row>
      <Table.Row>
        <Table.Cell>Contractor name:
        </Table.Cell>
        <Table.Cell>
       <div style={{color: selectedDocumentAttributes?.ContractorName == 'N/D' ? 'red':""}}>{selectedDocumentAttributes?.ContractorName}</div>
      </Table.Cell>
      </Table.Row>
      <Table.Row>
        <Table.Cell>Footer name:
        </Table.Cell>
        <Table.Cell>
          <div style={{color: selectedDocumentAttributes?.FooterName == 'N/D' ? 'red':""}}>{selectedDocumentAttributes?.FooterName}</div>
          </Table.Cell>
      </Table.Row>
      <Table.Row>
        <Table.Cell>Revision:</Table.Cell>
        <Table.Cell>
          <div style={{color: selectedDocumentAttributes?.Rev == 'N/D' ? 'red':""}}>{selectedDocumentAttributes?.Rev}</div>
          </Table.Cell>
      </Table.Row>
      <Table.Row>
        <Table.Cell>Issue:</Table.Cell>
        <Table.Cell>
          <div style={{color:selectedDocumentAttributes?.Issue == 'N/D' ? 'red':""}}>{selectedDocumentAttributes?.Issue}</div>
          </Table.Cell>
      </Table.Row>
      <Table.Row>
        <Table.Cell>PurposeIssue:</Table.Cell>
        <Table.Cell>
        <div style={{color:selectedDocumentAttributes?.PurposeIssue == 'N/D' ? 'red':""}}>{selectedDocumentAttributes?.PurposeIssue}</div>
        </Table.Cell>

      </Table.Row>
      <Table.Row>
        <Table.Cell>StageEn:</Table.Cell>
        <Table.Cell><div style={{color:selectedDocumentAttributes?.StageEn == 'N/D' ? 'red':""}}>{selectedDocumentAttributes?.StageEn}</div></Table.Cell>
      </Table.Row>
      <Table.Row>
        <Table.Cell>Sheet:</Table.Cell>
          <Table.Cell>
          <div style={{color:selectedDocumentAttributes?.Sheet == -1 ? 'red':""}}>{selectedDocumentAttributes?.Sheet}</div>
            </Table.Cell>
      </Table.Row>
      <Table.Row>
        <Table.Cell>Total sheets:</Table.Cell>
          <Table.Cell>
          <div style={{color:selectedDocumentAttributes?.TotalSheets == -1 ? 'red':""}}>{selectedDocumentAttributes?.TotalSheets}</div>
          </Table.Cell>
      </Table.Row>
      <Table.Row>
        <Table.Cell>Description eng:</Table.Cell>
          <Table.Cell>
          <div style={{color:selectedDocumentAttributes?.EngDescription == 'N/D' ? 'red':""}}>{selectedDocumentAttributes?.EngDescription}</div>
          </Table.Cell>
      </Table.Row>      
      <Table.Row>
        <Table.Cell>Description rus:</Table.Cell>
          <Table.Cell>
          <div style={{color:selectedDocumentAttributes?.RusDescription == 'N/D' ? 'red':""}}>{selectedDocumentAttributes?.RusDescription}</div>
            </Table.Cell>
      </Table.Row>
      <Table.Row>
        <Table.Cell>Client Rev:</Table.Cell>
          <Table.Cell>          
            <div style={{color:selectedDocumentAttributes?.ClientRev == 'N/D' ? 'red':""}}>{selectedDocumentAttributes?.ClientRev}</div>
          </Table.Cell>
      </Table.Row>
      <Table.Row>
        <Table.Cell>Date:</Table.Cell>
          <Table.Cell>
          <div style={{color:selectedDocumentAttributes?.Date == '0001-01-01T00:00:00'? 'red':""}}>{selectedDocumentAttributes?.Date.split('T')[0]}</div>
            </Table.Cell>
      </Table.Row>
    </Table.Body>
    </Table>
  )
}