import React from 'react';

import { Header, Flex, Button, Text, Loader } from "@fluentui/react-northstar";
import * as microsoftTeams from "@microsoft/teams-js";

import "./../styles.scss"

import { getApplauseCardAPI } from "./../../apis/ApplauseCardApi"

import Toggle from 'react-toggle'
// import ImageUploader from 'react-images-upload';

const base_URL = window.location.origin
const editImage = base_URL + "/images/edit.svg"
const backImage = base_URL + "/images/left-arrow.svg"
const upArrow = base_URL + "/images/upArrow.png"
const downArrow = base_URL + "/images/downArrow.png"

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
    badgeList?: any;
    base64Image?: any;
    loading?: any;
    addNewTaskModuleURL?: any;
    nameFieldSort?: any;
    nameFieldSortIcon?: any;
    activeFieldSort?: any;
    activeFieldSortIcon?: any
};


class Badge extends React.Component<IBadgeProps, MyState> {

    constructor(props: IBadgeProps) {
        super(props);
        this.state = {
            loading: true,
            addNewTaskModuleURL: base_URL+'/addbadge',
            nameFieldSort: true,
            nameFieldSortIcon: true
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
            // console.log("get ApplauseCard API", res);
            this.setState({
                badgeList: res.data.sort((a: any, b: any) => (a.cardName > b.cardName) ? 1 : ((b.cardName > a.cardName) ? -1 : 0)),
                base64Image: null,
                loading: false
            })

        })
    }





    addNewTaskModule = () => {
        let taskInfo: ITaskInfo = {
            url: this.state.addNewTaskModuleURL,
            title: "Create New Applaud Card",
            height: 350,
            width: 600,
            fallbackUrl: this.state.addNewTaskModuleURL,
        }
        let submitHandler = (err: any, result: any) => {
         this.getApplauseCard()
        };

        microsoftTeams.tasks.startTask(taskInfo, submitHandler);
    }

    editTaskModule = (data: any) => {
        let taskInfo: ITaskInfo = {
            url: `${base_URL}/editbadge?id=${data.cardId}`,
            title: "Edit Applaud Card",
            height: 350,
            width: 600,
            fallbackUrl: `${base_URL}/editbadge?id=${data.cardId}`,
        }
        let submitHandler = (err: any, result: any) => {
            this.setState({
                loading: true
            }, () => {
                this.getApplauseCard()
            })

        };

        microsoftTeams.tasks.startTask(taskInfo, submitHandler);
    }


    back() {
        this.props.history.push(`/admin_preview`)
    }

    ////////////////////// Name field sort ///////////////////////
    nameSort() {
        this.setState({
            activeFieldSort: false,
            nameFieldSortIcon: true,
            activeFieldSortIcon: false
        }, () => {
            if (this.state.nameFieldSort) {
                this.setState({
                    badgeList: this.state.badgeList.reverse((a: any, b: any) => (a.cardName > b.cardName) ? 1 : ((b.cardName > a.cardName) ? -1 : 0)),
                    nameFieldSort: false,
                })
            }
            else {
                this.setState({
                    badgeList: this.state.badgeList.sort((a: any, b: any) => (a.cardName > b.cardName) ? 1 : ((b.cardName > a.cardName) ? -1 : 0)),
                    nameFieldSort: true,
                })
            }
        })
    }

    ///////////////////////// Active field sort /////////////////////////////
    activeSort() {
        this.setState({
            nameFieldSort: false,
            nameFieldSortIcon: false,
            activeFieldSortIcon: true
        }, () => {
            if (this.state.activeFieldSort) {
                this.setState({
                    badgeList: this.state.badgeList.reverse((a: any, b: any) => b.isActive - a.isActive),
                    activeFieldSort: false,
                })
            }
            else {
                this.setState({
                    badgeList: this.state.badgeList.sort((a: any, b: any) => b.isActive - a.isActive),
                    activeFieldSort: true,
                })
            }
        })
    }



    render() {

        return (
            <div className="containterBox">
                <div>
                    <div className="displayFlex" style={{ alignItems: "center" }}>
                        <div className="backButton pointer" onClick={() => this.back()}>
                            <img src={backImage} />
                        </div>

                        <Header as="h3" content="Applaud Cards" className="headingText"></Header>
                        <div className="addNewDiv">
                            <Button primary content="+Add New" className="addNewButton" onClick={() => this.addNewTaskModule()} />
                        </div>
                    </div>

                </div>

                {!this.state.loading ? <table className="ViswasTable">
                    <tr>
                        <th>
                            <div className="displayFlex pointer" onClick={() => this.nameSort()}  >
                                <div style={{ marginRight: "5px" }}> Name </div>
                                {this.state.nameFieldSortIcon && <img style={{ marginTop: "3px" }} src={(this.state.nameFieldSort) ? downArrow : upArrow} />}
                            </div>
                        </th>
                        <th>Icon</th>
                        <th>
                            <div className="displayFlex pointer" style={{justifyContent:"center",paddingRight:"10px"}} onClick={() => this.activeSort()} >
                                <div style={{ marginRight: "5px"}}> Active </div>
                                {this.state.activeFieldSortIcon && <img style={{ marginTop: "3px" }} src={(this.state.activeFieldSort) ? downArrow : upArrow} />}
                            </div>
                        </th>
                        <th style={{ textAlign: "end", paddingRight: '45px' }}>Action</th>
                    </tr>
                    {this.state.badgeList && this.state.badgeList.map((e: any) => {
                        return <tr className="ViswasTableRow">
                            <td>{e.cardName}</td>
                            <td>
                                <div className="reportListImageDiv">
                                    <img src={e.cardImage} className="badgePageIcon" />
                                </div>
                            </td>
                            <td>
                                <Flex styles={{ alignItems: "center", justifyContent: "center" }}>
                                    <Toggle disabled={true} checked={e.isActive} icons={false} />
                                    <Text styles={{ marginLeft: "5px" }}>{e.isActive ? "Yes" : "No"}</Text>
                                </Flex>

                            </td>
                            <td style={{ textAlign: "end" }}>
                                <div className="tableEditDiv">
                                    <div style={{ marginRight: "10px" }}> Edit </div>
                                    <div className="editButton pointer" onClick={() => this.editTaskModule(e)}  ><img src={editImage} /></div>
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
