import { stringify } from 'querystring';
import React from 'react';
import {Icon, Label, Table } from 'semantic-ui-react';
import { AttributesModel } from '../models/AttributesModel';

interface Props{

  model: AttributesModel;
  }

  export default function AttributesTable({model}: Props)
{
  return (
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
        <div style={{color: model.Page == -1 ? 'red':""}}>{model.Page}</div>
      </Table.Cell>
    </Table.Row>
    <Table.Row>
      <Table.Cell>File name:</Table.Cell>
      <Table.Cell>
       <div style={{color: model.FileName == 'error' || "" ? 'red':""}}>{model.FileName}</div>
      </Table.Cell>
    </Table.Row>
      <Table.Row>
        <Table.Cell>Contractor name:
        </Table.Cell>
        <Table.Cell>
       <div style={{color: model.ContractorName == 'error' || "" ? 'red':""}}>{model.ContractorName}</div>
      </Table.Cell>
      </Table.Row>
      <Table.Row>
        <Table.Cell>Footer name:
        </Table.Cell>
        <Table.Cell>
          <div style={{color: model.FooterName == 'error' || "" ? 'red':""}}>{model.FooterName}</div>
          </Table.Cell>
      </Table.Row>
      <Table.Row>
        <Table.Cell>Revision:</Table.Cell>
        <Table.Cell>
          <div style={{color: model.Rev == 'error' || "" ? 'red':""}}>{model.Rev}</div>
          </Table.Cell>
      </Table.Row>
      <Table.Row>
        <Table.Cell>Issue:</Table.Cell>
        <Table.Cell>
          <div style={{color: model.Issue == 'error' || "" ? 'red':""}}>{model.Issue}</div>
          </Table.Cell>
      </Table.Row>
      <Table.Row>
        <Table.Cell>PurposeIssue:</Table.Cell>
        <Table.Cell>
        <div style={{color: model.PurposeIssue == 'error' || "" ? 'red':""}}>{model.PurposeIssue}</div>
        </Table.Cell>

      </Table.Row>
      <Table.Row>
        <Table.Cell>StageEn:</Table.Cell>
        <Table.Cell><div style={{color: model.StageEn == 'error' || "" ? 'red':""}}>{model.StageEn}</div></Table.Cell>

      </Table.Row>
      <Table.Row>
        <Table.Cell>StageRu:</Table.Cell>
          <Table.Cell>
            <div style={{color: model.StageRu == 'error' || "" ? 'red':""}}>{model.StageRu}</div>
            </Table.Cell>
      </Table.Row>
      <Table.Row>
        <Table.Cell>Sheet:</Table.Cell>
          <Table.Cell>
          <div style={{color: model.Sheet == -1 ? 'red':""}}>{model.Sheet}</div>
            </Table.Cell>
      </Table.Row>
      <Table.Row>
        <Table.Cell>Total sheets:</Table.Cell>
          <Table.Cell>
          <div style={{color: model.TotalSheets == -1 ? 'red':""}}>{model.TotalSheets}</div>
          </Table.Cell>
      </Table.Row>
      <Table.Row>
        <Table.Cell>Description eng:</Table.Cell>
          <Table.Cell>
          <div style={{color: model.EngDescription == 'error' || "" ? 'red':""}}>{model.EngDescription}</div>
          </Table.Cell>
      </Table.Row>      
      <Table.Row>
        <Table.Cell>Description rus:</Table.Cell>
          <Table.Cell>
          <div style={{color: model.RusDescription == 'error' || "" ? 'red':""}}>{model.RusDescription}</div>
            </Table.Cell>
      </Table.Row>
      <Table.Row>
        <Table.Cell>Client Rev:</Table.Cell>
          <Table.Cell>          
            <div style={{color: model.ClientRev == 'error' || "" ? 'red':""}}>{model.ClientRev}</div>
          </Table.Cell>
      </Table.Row>
      <Table.Row>
        <Table.Cell>Date:</Table.Cell>
          <Table.Cell>
          <div style={{color: model.Date == '0001-01-01T00:00:00'? 'red':""}}>{model.Date.split('T')[0]}</div>
            </Table.Cell>
      </Table.Row>
      <Table.Row>
        <Table.Cell>Pape Size:</Table.Cell>
          <Table.Cell>
          <div style={{color: model.PapeSize == 'error' || "" ? 'red':""}}>{model.PapeSize}</div>
            </Table.Cell>
      </Table.Row>
      <Table.Row>
        <Table.Cell>Status:</Table.Cell>
          <Table.Cell>
          <div style={{color: model.Status == 'error' || "" ? 'red':""}}>{model.Status}</div>
          </Table.Cell>
      </Table.Row>
      <Table.Row>
        <Table.Cell>Scale:</Table.Cell>
        <Table.Cell>
       <div style={{color: model.Scale == 'error' || "" ? 'red':""}}>{model.Scale}</div>
      </Table.Cell>
      </Table.Row>
    </Table.Body>
    </Table>
  )
}