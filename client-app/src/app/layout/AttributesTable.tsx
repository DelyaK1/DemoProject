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
        <Table.HeaderCell>Name attribute</Table.HeaderCell>
        <Table.HeaderCell>Value</Table.HeaderCell>
        <Table.HeaderCell>Status</Table.HeaderCell>
      </Table.Row>
    </Table.Header>

    <Table.Body>
    <Table.Row>
        <Table.Cell>Page:</Table.Cell>
        <Table.Cell>{model.Page}</Table.Cell>
        <Table.Cell textAlign='center' singleLine>          
            <Icon color='green' name='checkmark' size='large' />          
          </Table.Cell>
      </Table.Row>
      <Table.Row>
        <Table.Cell>File name:</Table.Cell>
        <Table.Cell>{model.FileName}</Table.Cell>
        <Table.Cell textAlign='center' singleLine>          
            <Icon color='green' name='checkmark' size='large' />          
          </Table.Cell>
      </Table.Row>
      <Table.Row>
        <Table.Cell>Contractor name:
        </Table.Cell>
        <Table.Cell>{model.ContractorName}</Table.Cell>
        <Table.Cell textAlign='center' singleLine>          
            <Icon color='green' name='checkmark' size='large' />          
          </Table.Cell>
      </Table.Row>
      <Table.Row>
        <Table.Cell>Footer name:
        </Table.Cell>
        <Table.Cell>{model.FooterName}</Table.Cell>
        <Table.Cell textAlign='center' singleLine>          
            <Icon color='red' name='close' size='large' />          
          </Table.Cell>
      </Table.Row>
      <Table.Row>
        <Table.Cell>Revision:</Table.Cell>
        <Table.Cell>{model.Rev}</Table.Cell>
        <Table.Cell textAlign='center' singleLine>          
            <Icon color='green' name='checkmark' size='large' />          
          </Table.Cell>
      </Table.Row>
      <Table.Row>
        <Table.Cell>Issue:</Table.Cell>
        <Table.Cell>{model.Issue}</Table.Cell>
        <Table.Cell textAlign='center' singleLine>          
            <Icon color='green' name='checkmark' size='large' />          
          </Table.Cell>
      </Table.Row>
      <Table.Row>
        <Table.Cell>PurposeIssue:</Table.Cell>
        <Table.Cell>{model.PurposeIssue}</Table.Cell>
        <Table.Cell textAlign='center' singleLine>          
            <Icon color='green' name='checkmark' size='large' />          
          </Table.Cell>
      </Table.Row>
      <Table.Row>
        <Table.Cell>StageEn:</Table.Cell>
        <Table.Cell>{model.StageEn}</Table.Cell>
        <Table.Cell textAlign='center' singleLine>          
            <Icon color='green' name='checkmark' size='large' />          
          </Table.Cell>
      </Table.Row>
      <Table.Row>
        <Table.Cell>StageRu:</Table.Cell>
        <Table.Cell>{model.StageRu}</Table.Cell>
        <Table.Cell textAlign='center' singleLine>          
            <Icon color='green' name='checkmark' size='large' />          
          </Table.Cell>
      </Table.Row>
      <Table.Row>
        <Table.Cell>Sheet:</Table.Cell>
        <Table.Cell>{model.Sheet}</Table.Cell>
        <Table.Cell textAlign='center' singleLine>          
            <Icon color='green' name='checkmark' size='large' />          
          </Table.Cell>
      </Table.Row>
      <Table.Row>
        <Table.Cell>Total sheets:</Table.Cell>
        <Table.Cell>{model.TotalSheets}</Table.Cell>
        <Table.Cell textAlign='center' singleLine>          
            <Icon color='green' name='checkmark' size='large' />          
          </Table.Cell>
      </Table.Row>
      <Table.Row>
        <Table.Cell>Description eng:</Table.Cell>
        <Table.Cell>{model.EngDescription}</Table.Cell>
        <Table.Cell textAlign='center' singleLine>          
            <Icon color='green' name='checkmark' size='large' />          
          </Table.Cell>
      </Table.Row>      
      <Table.Row>
        <Table.Cell>Description rus:</Table.Cell>
        <Table.Cell>{model.RusDescription}</Table.Cell>
        <Table.Cell textAlign='center' singleLine>          
            <Icon color='green' name='checkmark' size='large' />          
          </Table.Cell>
      </Table.Row>
      <Table.Row>
        <Table.Cell>Client Rev:</Table.Cell>
        <Table.Cell>{model.ClientRev}</Table.Cell>
        <Table.Cell textAlign='center' singleLine>          
            <Icon color='green' name='checkmark' size='large' />          
          </Table.Cell>
      </Table.Row>
      <Table.Row>
        <Table.Cell>Date:</Table.Cell>
        <Table.Cell>{model.Date.split('T')[0]}</Table.Cell>
        <Table.Cell textAlign='center' singleLine>          
            <Icon color='green' name='checkmark' size='large' />          
          </Table.Cell>
      </Table.Row>
      <Table.Row>
        <Table.Cell>Pape Size:</Table.Cell>
        <Table.Cell>{model.PapeSize}</Table.Cell>
        <Table.Cell textAlign='center' singleLine>          
            <Icon color='green' name='checkmark' size='large' />          
          </Table.Cell>
      </Table.Row>
      <Table.Row>
        <Table.Cell>Status:</Table.Cell>
        <Table.Cell>{model.Status}</Table.Cell>
        <Table.Cell textAlign='center' singleLine>          
            <Icon color='green' name='checkmark' size='large' />          
          </Table.Cell>
      </Table.Row>
      <Table.Row>
        <Table.Cell>Scale:</Table.Cell>
        <Table.Cell>{model.Scale}</Table.Cell>
        <Table.Cell textAlign='center' singleLine>          
            <Icon color='green' name='checkmark' size='large' />          
          </Table.Cell>
      </Table.Row>
    </Table.Body>
    </Table>
  )
}