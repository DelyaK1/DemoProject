import React from 'react'
import { List } from 'semantic-ui-react'
import { DocumentAttributesModel } from '../models/DocumentAttributesModel';
interface Props{

    selectedDocumentAttributes: DocumentAttributesModel | undefined;
    
  }
  export default function ListTemplate({selectedDocumentAttributes}: Props)
  {
    return (
<List as='ul'>
    <List.Item as='li'>{selectedDocumentAttributes?.name}</List.Item>
    <List.Item as='li'>Inviting Friends</List.Item>
    <List.Item as='li'>
      Benefits
      <List.List as='ul'>
        <List.Item as='li'>
          <a href='#'>Link to somewhere</a>
        </List.Item>
        <List.Item as='li'>Rebates</List.Item>
        <List.Item as='li'>Discounts</List.Item>
      </List.List>
    </List.Item>
    <List.Item as='li'>Warranty</List.Item>
  </List>
    )
  }
  
