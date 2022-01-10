import React from 'react';
import { Container, Menu } from 'semantic-ui-react';
import "/VS/AGCC_RD/DemoProject/client-app/src/styles/Header.css"

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