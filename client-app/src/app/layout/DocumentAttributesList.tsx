import React from 'react';
import {Item, Label, List, Table } from 'semantic-ui-react';
import { DocumentAttributesModel } from '../models/DocumentAttributesModel';
import AttributesTable from './AttributesTable';
interface Props{

    selectedDocumentAttributes: DocumentAttributesModel | undefined;
    
  }
export default function DocumentAttributesList({selectedDocumentAttributes}: Props)
{
    return (

    <Item.Group>
        <Item>
        <Item.Content>
        {/* <Item.Header>{selectedDocumentAttributes?.name}</Item.Header> */}
        {selectedDocumentAttributes?.models.map(model =>
        <>
        <AttributesTable model={model}/>
        </>
        )}
      </Item.Content>
        </Item>
    </Item.Group>            
    )
}