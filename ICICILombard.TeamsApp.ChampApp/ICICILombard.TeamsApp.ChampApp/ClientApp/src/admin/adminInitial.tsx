import React from 'react';

import {  Button } from "@fluentui/react-northstar";

import "./../styles.scss"
import * as microsoftTeams from "@microsoft/teams-js";

type MyState = {
};
 
const url=window.location.origin

class AdminInitial extends React.Component<MyState> {
    state: MyState = {
       
    };



    componentDidMount() {
        microsoftTeams.initialize();
    }

    submit(){
        console.log("submit")
        microsoftTeams.settings.setValidityState(true);
        this.save()
    }

     save(){
        console.log("save 1")
        microsoftTeams.settings.registerOnSaveHandler((saveEvent) => {
            console.log("save 2")
            microsoftTeams.settings.setSettings({
                websiteUrl: url,
                contentUrl: url+'/admin_preview',
                entityId: "adminTab",
                suggestedDisplayName: "Champ App Admin"
            });
            saveEvent.notifySuccess();
        });
    }

    render() {

        return (
            <div style={{
                display:"flex",
                justifyContent:"center",
                height:"100%"

            }}>
                <Button primary onClick={()=>this.submit()}>Configure</Button>

            </div>
        );
    }
}



export default AdminInitial;
