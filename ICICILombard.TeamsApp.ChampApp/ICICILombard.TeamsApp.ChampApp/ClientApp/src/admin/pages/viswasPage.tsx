import React from 'react';

import { Header, Flex, Button, Text, Loader } from "@fluentui/react-northstar";
import * as microsoftTeams from "@microsoft/teams-js";
import "./../styles.scss"

import { getViswasBehaviourAPI } from './../../apis/ViswasBehaviourApi'

import Toggle from 'react-toggle'


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

interface IViswasProps {
    history?: any;
    location?: any
}


interface MyState {
    ViswasBehaviourList?: any;
    loading?: any;
    url?: any
}


class Vishvas extends React.Component<IViswasProps, MyState> {
    constructor(props: IViswasProps) {
        super(props);
        this.state = {
            loading: true,
            url: base_URL + '/addviswas'
        };
    }


    componentDidMount() {
        this.getViswasBehaviour()
    }


    getViswasBehaviour() {
        const data = {
            // "IsActive": 1,
            "BehaviourId": 0
        }
        getViswasBehaviourAPI(data).then((res) => {
            console.log("api visws get", res.data);
            this.setState({
                ViswasBehaviourList: res.data,
                loading: false
            })

        })
    }



    addNewTaskModule = () => {
        let taskInfo: ITaskInfo = {
            url: this.state.url,
            title: "Create New Values/Behaviors ",
            height: 350,
            width: 600,
            fallbackUrl: this.state.url,
        }
        console.log("task module", taskInfo);
        let submitHandler = (err: any, result: any) => {
            console.log("errr", err);
            console.log("result", result);
            this.getViswasBehaviour()
        };

        microsoftTeams.tasks.startTask(taskInfo, submitHandler);
    }

    editTaskModule = (data: any) => {
        let taskInfo: ITaskInfo = {
            url: `${base_URL}/editviswas?id=${data.behaviourId}`,
            title: "Edit Values/Behaviors ",
            height: 350,
            width: 600,
            fallbackUrl: this.state.url,
        }
        console.log("task module", taskInfo);
        let submitHandler = (err: any, result: any) => {
            this.setState({
                loading:true
            },()=>{
                this.getViswasBehaviour()
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

                    <div className="displayFlex " style={{ alignItems: "center" }}>
                        <div className="backButton pointer" onClick={() => this.back()}>
                        <img src={backImage.default}/>
                        </div>

                        <Header as="h3" content=" Values/Behaviors" className="headingText"></Header>
                        <div className="addNewDiv">
                            <Button primary content="+Add New" className="addNewButton" onClick={() => this.addNewTaskModule()} />
                            {/* <Dialog
                                cancelButton="Cancel"
                                confirmButton="+ Add"
                                content={{
                                    children: (Component, props) => {
                                        const { styles, ...rest } = props
                                        return (
                                            <div style={{ paddingBottom: "60px" }}>
                                                <div className="displayFlex">
                                                    <img src="https://image.freepik.com/free-vector/abstract-logo-flame-shape_1043-44.jpg" className="logoIcon" />
                                                    <div className="displayFlex logoText">
                                                        <Text size="large" weight="bold">Vishvas Behaviours</Text>
                                                        <Text size="medium">Create New</Text>
                                                    </div>
                                                </div>
                                                <Divider />
                                                <Card fluid styles={{
                                                    display: 'block',
                                                    backgroundColor: 'white',
                                                    padding: '0',
                                                    marginTop:"10px",
                                                    ':hover': {
                                                        backgroundColor: 'white',
                                                    },
                                                }}>
                                                    <CardBody>

                                                        <Form styles={{
                                                            paddingTop: '20px'
                                                        }}>
                                                            <FormInput
                                                                label="Name"
                                                                name="Name"
                                                                id="Name"
                                                                required fluid
                                                                onChange={(e) => this.addNewInput(e)}
                                                                showSuccessIndicator={false}
                                                            />
                                                        </Form>
                                                    </CardBody>

                                                </Card>
                                            </div>
                                        )
                                    },
                                }}
                                onConfirm={() => this.addNew()}
                                trigger={<Button primary content="+Add New" className="addNewButton" />}
                            /> */}
                        </div>

                    </div>



                </div>
                {!this.state.loading ? <div>
                    {this.state.ViswasBehaviourList ? <table className="ViswasTable">
                        <tr>
                            <th>Name</th>
                            <th>Active</th>
                            <th style={{ textAlign: "end", paddingRight: '45px' }}>Action</th>
                        </tr>
                        {this.state.ViswasBehaviourList.map((e: any) => {
                            return <tr className="ViswasTableRow">
                                <td>{e.behaviourName}</td>
                                <td>
                                    <Flex styles={{ alignItems: "center" }}>
                                        <Toggle disabled={true} defaultChecked={e.isActive ? true:false} icons={false} />
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


                    </table> : <div className="noDataText"> No Data Available</div>}

                </div> : <Loader styles={{ margin: "50px" }} />}

            </div>
        );
    }
}



export default Vishvas;
