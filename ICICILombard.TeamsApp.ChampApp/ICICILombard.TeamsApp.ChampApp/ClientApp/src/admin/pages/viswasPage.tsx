import React from 'react';

import { Header, Flex, Button, Text, Loader } from "@fluentui/react-northstar";
import * as microsoftTeams from "@microsoft/teams-js";
import "./../styles.scss"

import { getViswasBehaviourAPI } from './../../apis/ViswasBehaviourApi'

import Toggle from 'react-toggle'

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

interface IViswasProps {
    history?: any;
    location?: any
}


interface MyState {
    viswasBehaviourList?: any;
    loading?: any;
    addNewTaskModuleURL?: any;
    nameFieldSort?: any;
    nameFieldSortIcon?: any;
    activeFieldSort?: any;
    activeFieldSortIcon?: any
}


class Vishvas extends React.Component<IViswasProps, MyState> {
    constructor(props: IViswasProps) {
        super(props);
        this.state = {
            loading: true,
            addNewTaskModuleURL: base_URL + '/addviswas',
            nameFieldSort: true,
            nameFieldSortIcon: true
        };
    }


    componentDidMount() {
        this.getViswasBehaviour()
    }

    ///////////////////////////// Get Values/Behavior list function ////////////////////////////
    getViswasBehaviour() {
        const data = {
            // "IsActive": 1,
            "BehaviourId": 0
        }
        getViswasBehaviourAPI(data).then((res) => {
            // console.log("api visws get", res.data);
            this.setState({
                viswasBehaviourList: res.data.sort((a: any, b: any) => (a.behaviourName > b.behaviourName) ? 1 : ((b.behaviourName > a.behaviourName) ? -1 : 0)),
                loading: false
            })
        })
    }


    /////////////////////// Call Add New Task module //////////////////////////
    addNewTaskModule = () => {
        let taskInfo: ITaskInfo = {
            url: this.state.addNewTaskModuleURL,
            title: "Create New Values/Behaviors ",
            height: 350,
            width: 600,
            fallbackUrl: this.state.addNewTaskModuleURL,
        }
        let submitHandler = (err: any, result: any) => {
            this.getViswasBehaviour()
        };
        microsoftTeams.tasks.startTask(taskInfo, submitHandler);
    }

    /////////////////////// Call Edit Task module //////////////////////////
    editTaskModule = (data: any) => {
        let taskInfo: ITaskInfo = {
            url: `${base_URL}/editviswas?id=${data.behaviourId}`,
            title: "Edit Values/Behaviors ",
            height: 350,
            width: 600,
            fallbackUrl: `${base_URL}/editviswas?id=${data.behaviourId}`,
        }
        let submitHandler = (err: any, result: any) => {
            this.setState({
                loading: true
            }, () => {
                this.getViswasBehaviour()
            })
        };

        microsoftTeams.tasks.startTask(taskInfo, submitHandler);
    }

    ////////////////// Back Button //////////////////////
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
                    viswasBehaviourList: this.state.viswasBehaviourList.reverse((a: any, b: any) => (a.behaviourName > b.behaviourName) ? 1 : ((b.behaviourName > a.behaviourName) ? -1 : 0)),
                    nameFieldSort: false,
                })
            }
            else {
                this.setState({
                    viswasBehaviourList: this.state.viswasBehaviourList.sort((a: any, b: any) => (a.behaviourName > b.behaviourName) ? 1 : ((b.behaviourName > a.behaviourName) ? -1 : 0)),
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
                    viswasBehaviourList: this.state.viswasBehaviourList.reverse((a: any, b: any) => b.isActive - a.isActive),
                    activeFieldSort: false,
                })
            }
            else {
                this.setState({
                    viswasBehaviourList: this.state.viswasBehaviourList.sort((a: any, b: any) => b.isActive - a.isActive),
                    activeFieldSort: true,
                })
            }
        })
    }

    render() {

        return (
            <div className="containterBox">
                <div>

                    <div className="displayFlex " style={{ alignItems: "center" }}>
                        <div className="backButton pointer" onClick={() => this.back()}>
                            <img src={backImage} />
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
                    {this.state.viswasBehaviourList ? <table className="ViswasTable">
                        <tr>
                            <th>
                                <div className="displayFlex pointer" onClick={() => this.nameSort()}  >
                                    <div style={{ marginRight: "5px" }}> Name </div>
                                    {this.state.nameFieldSortIcon && <img style={{ marginTop: "3px" }} src={(this.state.nameFieldSort) ? downArrow : upArrow} />}
                                </div>
                            </th>
                            <th>
                                <div className="displayFlex pointer" onClick={() => this.activeSort()} >
                                    <div style={{ marginRight: "5px" }}> Active </div>
                                    {this.state.activeFieldSortIcon && <img style={{ marginTop: "3px" }} src={(this.state.activeFieldSort) ? downArrow : upArrow} />}
                                </div>
                            </th>
                            <th style={{ textAlign: "end", paddingRight: '45px' }}>Action</th>
                        </tr>
                        {this.state.viswasBehaviourList.map((e: any) => {
                            return <tr className="ViswasTableRow">
                                <td>{e.behaviourName}</td>
                                <td>
                                    <Flex styles={{ alignItems: "center" }}>
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


                    </table> : <div className="noDataText"> No Data Available</div>}

                </div> : <Loader styles={{ margin: "50px" }} />}

            </div>
        );
    }
}



export default Vishvas;
