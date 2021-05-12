import React from 'react';
import { EditIcon, ArrowLeftIcon } from '@fluentui/react-icons-northstar';
import { Header, Divider, Flex, Button, Card, CardBody, FormInput, Form, Dialog, Text } from "@fluentui/react-northstar";

import "./../styles.scss"

import { getViswasBehaviourAPI, addViswasBehaviourAPI } from './../../apis/ViswasBehaviourApi'

import Toggle from 'react-toggle'


interface IViswasProps {
    history?: any;
    location?: any
}


interface MyState {
    ViswasBehaviourList?: any;
    addNewInputName?: any;
    editActiveStatusValue?: any;
    editNameValue?: any;
    editActiveStatus?: any
}


class Vishvas extends React.Component<IViswasProps,MyState> {
    constructor(props: IViswasProps) {
        super(props);
        this.state = {
            editActiveStatus:false
        };
    }

    // state: MyState = {
    //     // ViswasBehaviourList: [
    //     //     { behaviourId: 1, behaviourName: "Walking Together", isActive: 1 },
    //     //     { behaviourId: 2, behaviourName: "Value 1", isActive: 0 }
    //     // ]
    //     editActiveStatus:false
    // };


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
                ViswasBehaviourList: res.data
            })

        })
    }


    addViswasBehaviour(data:any){
        addViswasBehaviourAPI(data).then((res)=>{
            console.log("add viswas",res.data);
            if(res.data.successFlag===1){
                this.getViswasBehaviour()
            }    
        })
    }


    addNewInput(event: any) {
        this.setState({
            addNewInputName: event.target.value
        })
    }

    addNew() {
        if (this.state.addNewInputName) {
            const data = {
                "BehaviourId": 0,
                "BehaviourName": this.state.addNewInputName,
                "IsActive": 1
            }
            this.addViswasBehaviour(data)
            // console.log(data)
            // this.setState({
            //     ViswasBehaviourList: [...this.state.ViswasBehaviourList, { BehaviourId: this.state.ViswasBehaviourList.length + 1, BehaviourName: this.state.addNewInputName, IsActive: true }]
            // }, () => {
            //     console.log('add new', this.state.ViswasBehaviourList);
            // })
        }
        else {
            alert("Name is required")
        }
    }

    editActiveStatus = (e: any) => {
        this.setState({
            editActiveStatusValue: e.target.checked ? 1 : 0,
            editActiveStatus:true
        }, () => {
            console.log(this.state.editActiveStatusValue);

        })
    }

    editName(e: any) {
        this.setState({
            editNameValue: e.target.value
        })
    }

    editFunction(data: any) {
        const Value = {
            "BehaviourId": data.behaviourId,
            "BehaviourName": this.state.editNameValue ? this.state.editNameValue : data.behaviourName,
            "IsActive": this.state.editActiveStatus ? this.state.editActiveStatusValue : data.isActive
        }
        this.addViswasBehaviour(Value)
        console.log("check", Value)
    }

    back(){
        this.props.history.push(`/admin_preview`)
    }

    render() {

        return (
            <div className="containterBox">
                <div>
                    <div className="displayFlex">
                    <Button onClick={() => this.back()} icon={<ArrowLeftIcon />} text />
                    <Header as="h2" content=" Vishvas Behaviours" style={{ margin: '0', fontWeight: 'lighter' }}></Header>
                    </div>
                

                    <Dialog
                        cancelButton="Cancel"
                        confirmButton="+ Add"
                        content={{
                            children: (Component, props) => {
                                const { styles, ...rest } = props
                                return (
                                    <div style={{ marginBottom: "30px" }}>
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
                    />
                </div>


                {this.state.ViswasBehaviourList ? <table className="ViswasTable">
                    <tr>
                        <th>Name</th>
                        <th>Active</th>
                        <th style={{ textAlign: "end", paddingRight: '25px' }}>Action</th>
                    </tr>
                    {this.state.ViswasBehaviourList.map((e: any) => {
                        return <tr className="ViswasTableRow">
                            <td>{e.behaviourName}</td>
                            <td>
                                <Flex styles={{ alignItems: "center" }}>
                                    <Toggle disabled={true} defaultChecked={e.isActive} icons={false} />
                                    <Text styles={{ marginLeft: "5px" }}>{e.isActive ? "Yes" : "No"}</Text>
                                </Flex>

                            </td>
                            <td style={{ textAlign: "end" }}>
                                <div style={{marginRight:"10px"}}> <Dialog
                                    cancelButton="Cancel"
                                    confirmButton="+ Add"
                                    content={{
                                        children: (Component, props) => {
                                            return (
                                                <div style={{ marginBottom: "40px" }}>
                                                    <div className="displayFlex">
                                                        <img src="https://image.freepik.com/free-vector/abstract-logo-flame-shape_1043-44.jpg" className="logoIcon" />
                                                        <div className="displayFlex logoText">
                                                            <Text size="large" weight="bold">Vishvas Behaviours</Text>
                                                            <Text size="medium">Edit </Text>
                                                        </div>
                                                    </div>
                                                    <Divider />
                                                    <div style={{ paddingTop: '20px' }}>
                                                        <FormInput
                                                            label="Name"
                                                            name="Name"
                                                            id="Name"
                                                            defaultValue={e.behaviourName}
                                                            required fluid
                                                            onChange={(e) => this.editName(e)}
                                                            showSuccessIndicator={false}
                                                        />
                                                        <div className="outerDivToggleRadioGroup">
                                                            <Text styles={{ marginRight: "5px" }}> Active Status </Text>
                                                            <Toggle defaultChecked={(e.isActive === 1) ? true : false} icons={false} onChange={(e) => this.editActiveStatus(e)} ></Toggle>
                                                            {/* <RadioGroup
                                                            defaultCheckedValue={e.active ? 1 : 2}
                                                            onCheckedValueChange={this.editActiveStatus}
                                                            items={[
                                                                { label: 'True', value: 1 },
                                                                { label: 'False', value: 2 },
                                                            ]}
                                                        /> */}
                                                        </div>

                                                    </div>
                                                </div>
                                            )
                                        },
                                    }}
                                    onConfirm={() => this.editFunction(e)}
                                    trigger={<Button icon={<EditIcon />} text iconOnly >Edit</Button>}
                                /></div>
                            </td>
                        </tr>
                    })}


                </table> : <div style={{marginTop:"25px"}}>No data Available</div>}

            </div>
        );
    }
}



export default Vishvas;
