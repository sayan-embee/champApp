import React from 'react';

import {  Button } from "@fluentui/react-northstar";

import "./../styles.scss"
import * as microsoftTeams from "@microsoft/teams-js";

type MyState = {
};
 
const url=window.location.origin

class ReportInitial extends React.Component<MyState> {
    state: MyState = {
       
    };



    componentDidMount() {
        microsoftTeams.initialize();
    }

    submit(){
        microsoftTeams.settings.setValidityState(true);
        this.save()
    }

     save(){
        microsoftTeams.settings.registerOnSaveHandler((saveEvent) => {
            microsoftTeams.settings.setSettings({
                websiteUrl: url,
                contentUrl: url+'/admin_award',
                entityId: "reportTab",
                suggestedDisplayName: "Rewards and Recognitions"
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



export default ReportInitial;
