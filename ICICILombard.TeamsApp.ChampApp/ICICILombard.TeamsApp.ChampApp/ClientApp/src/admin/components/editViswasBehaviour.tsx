import React from 'react';
import { Text, FormInput, FlexItem, Loader, Flex, Button } from "@fluentui/react-northstar";

import * as microsoftTeams from "@microsoft/teams-js";

import "./../styles.scss"
import { addViswasBehaviourAPI, getViswasBehaviourAPI } from './../../apis/ViswasBehaviourApi'

import Toggle from 'react-toggle'

interface MyState {
    behaviourId?: any;
    viswasBehaviourList?: any;
    editActiveStatusValue?: any;
    editNameValue?: any;
    editActiveStatus?: any;
    loading?: any;
    buttonDisabled?: any
}

interface IViswasEditProps {
    history?: any;
    location?: any
}

class EditViswas extends React.Component<IViswasEditProps, MyState> {
    constructor(props: IViswasEditProps) {
        super(props);
        this.state = {
            loading: true,
            editActiveStatus: false,
            buttonDisabled: false
        };
    }



    componentDidMount() {
        const params = new URLSearchParams(this.props.location.search);
        this.setState({
            behaviourId: params.get('id'),
        },()=>{
            this.getViswasBehaviour()
        })
    }

    getViswasBehaviour() {
        const data = {
            "BehaviourId": this.state.behaviourId
        }
        getViswasBehaviourAPI(data).then((res) => {
            this.setState({
                viswasBehaviourList: res.data,
                loading: false
            })

        })
    }

    editActiveStatus = (e: any) => {
        this.setState({
            editActiveStatusValue: e.target.checked ? 1 : 0,
            editActiveStatus: true
        })
    }

    editName(e: any) {
        if (e.target.value.length === 0) {
            this.setState({
                buttonDisabled: true
            })
        }
        else{
            this.setState({
                buttonDisabled: false
            })
        }      
        this.setState({
            editNameValue: e.target.value
        })
    }


    addViswasBehaviour(data: any) {
        addViswasBehaviourAPI(data).then((res) => {
            // console.log("add viswas", res.data);
            if (res.data.successFlag === 1) {
                microsoftTeams.tasks.submitTask()
            }
        })
    }

    editFunction(data:any) {
        const Value = {
            "BehaviourId": data.behaviourId,
            "BehaviourName": this.state.editNameValue ? this.state.editNameValue : data.behaviourName,
            "IsActive": this.state.editActiveStatus ? this.state.editActiveStatusValue : data.isActive
        }
        this.addViswasBehaviour(Value)
    }


    render() {
        return (
            <div style={{ margin: "25px", height: "200px" }}>
                {!this.state.loading ? <div>
                    {this.state.viswasBehaviourList && <div>
                    {this.state.viswasBehaviourList.map((e:any)=>{
                            return <div style={{ paddingTop: '20px', marginTop: "10px" }}>
                             <FormInput
                                label="Name"
                                name="Name"
                                id="Name"
                                defaultValue={e.behaviourName}
                                required fluid
                                onChange={(e) => this.editName(e)}
                                showSuccessIndicator={false}
                            />
                             <div className="outerDivToggleRadioGroup editOuterDivToggleRadioGroup">
                                <Text styles={{ marginRight: "10px" }}> Active</Text>
                                <Toggle defaultChecked={e.isActive} icons={false} onChange={(e) => this.editActiveStatus(e)} ></Toggle>
                            </div>
        
                        
                            <div className="taskmoduleButtonDiv">
                                <Flex gap="gap.small">
                                    <FlexItem push>
                                        <Button disabled={this.state.buttonDisabled?true:false} primary onClick={() => this.editFunction(e)}>
                                            Update
                                        </Button>
                                    </FlexItem>
                                </Flex>
                            </div>
                            </div>
                    })}
                    </div>}
                </div> : <Loader styles={{ margin: "50px" }} />}

            </div>
        );
    }
}



export default EditViswas;
