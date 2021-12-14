import React from 'react';
import {Button, Container, Menu} from 'semantic-ui-react';

// interface Props{
//     openForm: ()=> void;
// }

export default function NavBar()
{
    return (
        <Menu inverted fixed='top'>
            <Container>
                <Menu.Item header>
                    File Manager
                </Menu.Item>
            </Container>
        </Menu>
    )
};