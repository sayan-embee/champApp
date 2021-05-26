import React from 'react';

import { Header, Flex, Button, Text, Loader } from "@fluentui/react-northstar";
import * as microsoftTeams from "@microsoft/teams-js";

import "./../styles.scss"

import { getApplauseCardAPI } from "./../../apis/ApplauseCardApi"

import Toggle from 'react-toggle'
// import ImageUploader from 'react-images-upload';

const base_URL = window.location.origin
const editImage = require("./../../assets/edit.svg")
const backImage = require("./../../assets/left-arrow.svg")

interface ITaskInfo {
    title?: string;
    height?: number;
    width?: number;
    url?: string;
    card?: string;
    fallbackUrl?: string;
    completionBotId?: string;
}

interface IBadgeProps {
    history?: any;
    location?: any
}

interface MyState {
    BadgeList?: any;
    base64Image?: any;
    loading?: any;
    url?: any
};


class Badge extends React.Component<IBadgeProps, MyState> {

    constructor(props: IBadgeProps) {
        super(props);
        this.state = {
            loading: true,
            url: base_URL + '/addbadge'
        };
    }


    componentDidMount() {
        this.getApplauseCard()
    }

    getApplauseCard() {
        const data = {
            "CardId": 0
        }
        getApplauseCardAPI(data).then((res) => {
            console.log("get ApplauseCard API", res);
            this.setState({
                BadgeList: res.data,
                base64Image: null,
                loading: false
            })

        })
    }





    addNewTaskModule = () => {
        let taskInfo: ITaskInfo = {
            url: this.state.url,
            title: "Create New Applaud Card",
            height: 350,
            width: 600,
            fallbackUrl: this.state.url,
        }
        console.log("task module", taskInfo);
        let submitHandler = (err: any, result: any) => {
            console.log("errr", err);
            console.log("result", result);
            this.getApplauseCard()
        };

        microsoftTeams.tasks.startTask(taskInfo, submitHandler);
    }

    editTaskModule = (data: any) => {
        let taskInfo: ITaskInfo = {
            url: `${base_URL}/editbadge?id=${data.cardId}&name=${data.cardName}&active=${data.isActive}`,
            title: "Edit Applaud Card",
            height: 350,
            width: 600,
            fallbackUrl: this.state.url,
        }
        console.log("task module", taskInfo);
        let submitHandler = (err: any, result: any) => {
            this.setState({
                loading:true
            },()=>{
                this.getApplauseCard()
            })
            
        };

        microsoftTeams.tasks.startTask(taskInfo, submitHandler);
    }


    back() {
        this.props.history.push(`/admin_preview`)
    }


    render() {

        return (
            <div className="containterBox">
                <div>
                    <div className="displayFlex" style={{ alignItems: "center" }}>
                        <div className="backButton pointer" onClick={() => this.back()}>
                            <img src={backImage.default}/>
                        </div>

                        <Header as="h3" content="Applaud Cards" className="headingText"></Header>
                        <div className="addNewDiv">
                            <Button primary content="+Add New" className="addNewButton" onClick={() => this.addNewTaskModule()} />
                        </div>
                    </div>

                </div>

                {!this.state.loading ? <table className="ViswasTable">
                    <tr>
                        <th>Name</th>
                        <th>Icon</th>
                        <th style={{ textAlign: "center", paddingRight: '25px' }}>Active</th>
                        <th style={{ textAlign: "end", paddingRight: '45px' }}>Action</th>
                    </tr>
                    {this.state.BadgeList && this.state.BadgeList.map((e: any) => {
                        return <tr className="ViswasTableRow">
                            <td>{e.cardName}</td>
                            <td>
                            <div className="reportListImageDiv">
                                <img src={e.cardImage} className="badgePageIcon" />
                                </div>
                                </td>
                            <td>
                                <Flex styles={{ alignItems: "center", justifyContent: "center" }}>
                                    <Toggle disabled={true} defaultChecked={(e.isActive === 1) ? true : false} icons={false} />
                                    <Text styles={{ marginLeft: "5px" }}>{e.isActive ? "Yes" : "No"}</Text>
                                </Flex>

                            </td>
                            <td style={{ textAlign: "end" }}>
                                <div className="tableEditDiv">
                                    <div style={{ marginRight: "10px" }}> Edit </div>
                                    <div className="editButton pointer" onClick={() => this.editTaskModule(e)}  ><img src={editImage.default} /></div>
                                </div>
                            </td>
                        </tr>
                    })}


                </table> : <Loader styles={{ margin: "50px" }} />}

            </div>
        );
    }
}



export default Badge;
