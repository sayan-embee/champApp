import React from 'react';
import {  Flex, Button, Card, CardBody, FormInput, Form, FlexItem } from "@fluentui/react-northstar";

import * as microsoftTeams from "@microsoft/teams-js";

import "./../styles.scss"
import { addViswasBehaviourAPI } from './../../apis/ViswasBehaviourApi'



interface MyState {
    addNewInputName?: any;
    loading?: any;
}

interface IViswasAddProps {
}

class AddViswas extends React.Component<IViswasAddProps, MyState> {
    constructor(props: IViswasAddProps) {
        super(props);
        this.state = {
        };
    }



    componentDidMount() {
    }


    addNewInput(event: any) {
        this.setState({
            addNewInputName: event.target.value
        })
    }

    addNew() {
            const data = {
                "BehaviourId": 0,
                "BehaviourName": this.state.addNewInputName,
                "IsActive": 1
            }
            this.addViswasBehaviour(data)
    }

    addViswasBehaviour(data: any) {
        addViswasBehaviourAPI(data).then((res) => {
            // console.log("add viswas", res.data);
            if (res.data.successFlag === 1) {
                microsoftTeams.tasks.submitTask()
            }
        })
    }


    render() {

        return (
            <div style={{ margin: "25px",height:"150px" }}>
                <Card fluid styles={{
                    display: 'block',
                    backgroundColor: 'transparent',
                    padding: '0',
                    marginTop: "10px",
                    ':hover': {
                        backgroundColor: 'transparent',
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
                                fluid
                                onChange={(e) => this.addNewInput(e)}
                                showSuccessIndicator={false}
                            />
                        </Form>
                    </CardBody>

                </Card>
                <div className="taskmoduleButtonDiv">
                <Flex gap="gap.small">
                <FlexItem push>
                  <Button disabled={this.state.addNewInputName?false:true}primary onClick={() => this.addNew()}>
                   Add New
                  </Button>
                </FlexItem>
              </Flex>
                </div>
                
            </div>
        );
    }
}



export default AddViswas;
