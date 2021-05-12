import React from 'react';
import { EditIcon, PaperclipIcon, ArrowLeftIcon } from '@fluentui/react-icons-northstar';
import { Header, Input, Flex, Button, Card, CardBody, FormInput, Form, Dialog, Text, Divider } from "@fluentui/react-northstar";

import "./../styles.scss"

import { getApplauseCardAPI, addApplauseCardAPI } from "./../../apis/ApplauseCardApi"

import Toggle from 'react-toggle'
// import ImageUploader from 'react-images-upload';




interface IBadgeProps {
    history?: any;
    location?: any
}

interface MyState {
    BadgeList?: any;
    addNewInputName?: any;
    editActiveStatusValue?: any;
    editNameValue?: any;
    base64Image?: any;
    editActiveStatus?: any
};


class Badge extends React.Component<IBadgeProps, MyState> {

    constructor(props: IBadgeProps) {
        super(props);
        this.state = {
            editActiveStatus: false
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
                base64Image: null
            })

        })
    }


    addApplauseCard(data: any) {
        addApplauseCardAPI(data).then((res) => {
            if (res.data.successFlag === 1) {
                this.getApplauseCard()
            }

        })
    }



    addNewInput(event: any) {
        this.setState({
            addNewInputName: event.target.value
        })
    }

    addNew() {
        if (this.state.addNewInputName && this.state.base64Image) {
            const data = {
                "CardId": 0,
                "CardName": this.state.addNewInputName,
                "CardImage": this.state.base64Image,
                "IsActive": 1

            }
            this.addApplauseCard(data)
            console.log("addNew badge", data);

        }
        else {
            alert("All the fields are required")
        }
    }

    editActiveStatus = (e: any) => {
        this.setState({
            editActiveStatusValue: e.target.checked ? 1 : 0,
            editActiveStatus: true
        })
    }


    editName(e: any) {
        this.setState({
            editNameValue: e.target.value
        })
    }

    editFunction(data: any) {
        const Value = {
            "CardId": data.cardId,
            "CardName": this.state.editNameValue ? this.state.editNameValue : data.cardName,
            "CardImage": this.state.base64Image ? this.state.base64Image : data.cardImage,
            "IsActive": this.state.editActiveStatus ? this.state.editActiveStatusValue : data.isActive
        }
        this.addApplauseCard(Value)
        console.log("check", data);

    }
    fileUpload() {
        (document.getElementById('upload') as HTMLInputElement).click()
    };

    onFileChoose(event: any) {
        console.log("check", event.target.files[0], event.target.files[0].lastModified);
        this.getBase64(event.target.files[0], event.target.files[0].lastModified)
    }

    getBase64(file: any, name: any) {
        var reader = new FileReader();
        reader.readAsDataURL(file);
        reader.onload = () => {
            console.log('Photo', reader.result);
            this.setState({
                base64Image: reader.result,
            })
        };
        reader.onerror = function (error) {
            console.log('Error: ', error);
        };
    }


    cancel(){
        this.setState({
            base64Image:null
        })
    }
    back() {
        this.props.history.push(`/admin_preview`)
    }


    render() {

        return (
            <div className="containterBox">
                <div>
                    <div className="displayFlex">
                        <Button onClick={() => this.back()} icon={<ArrowLeftIcon />} text content="Back" />

                        <Header as="h2" content="Applaud Cards" style={{ margin: '0', fontWeight: 'lighter' }}></Header>
                    </div>
                    <Dialog
                        cancelButton="Cancel"
                        confirmButton="+ Add"
                        content={{
                            children: (Component, props) => {
                                const { styles } = props
                                return (
                                    <div style={{ marginBottom: "30px" }}>
                                        <div className="displayFlex">
                                            <img src="https://image.freepik.com/free-vector/abstract-logo-flame-shape_1043-44.jpg" className="logoIcon" />
                                            <div className="displayFlex logoText">
                                                <Text size="large" weight="bold">Applaud Cards</Text>
                                                <Text size="medium">Create New</Text>
                                            </div>
                                        </div>
                                        <Divider />
                                        <Card fluid styles={{
                                            display: 'block',
                                            backgroundColor: 'white',
                                            padding: '0',
                                            marginBottom: '40px',
                                            ':hover': {
                                                backgroundColor: 'white',
                                            },
                                        }}>
                                            <CardBody>

                                                <Form styles={{
                                                    paddingTop: '25px',
                                                    marginBottom: "10px"
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
                                            <div style={{ display: "flex", flexDirection: "column" }}>
                                                <Text> Attach Icon</Text>
                                                <Input type="file" id="upload" style={{ display: 'none' }} onChange={value => this.onFileChoose(value)}></Input>

                                                <Button onClick={() => this.fileUpload()} className='cameraBtn'>
                                                    <PaperclipIcon /> Upload Photo
                                                </Button>
                                            </div>
                                            {this.state.base64Image && <img src={this.state.base64Image} className="badgePageEditIcon" />}
                                        </Card>
                                    </div>
                                )
                            },
                        }}
                        onConfirm={() => this.addNew()}
                        onCancel={()=>this.cancel()}
                        trigger={<Button primary content="+Add New" className="addNewButton" />}
                    />
                </div>

                <table className="ViswasTable">
                    <tr>
                        <th>Name</th>
                        <th>Icon</th>
                        <th style={{ textAlign: "center", paddingRight: '25px' }}>Active</th>
                        <th style={{ textAlign: "end", paddingRight: '25px' }}>Action</th>
                    </tr>
                    {this.state.BadgeList && this.state.BadgeList.map((e: any) => {
                        return <tr className="ViswasTableRow">
                            <td>{e.cardName}</td>
                            <td><img src={e.cardImage} className="badgePageIcon" /></td>
                            <td>
                                <Flex styles={{ alignItems: "center", justifyContent: "center" }}>
                                    <Toggle disabled={true} defaultChecked={(e.isActive === 1) ? true : false} icons={false} />
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
                                                            <Text size="large" weight="bold">Applaud Cards</Text>
                                                            <Text size="medium">Editw</Text>
                                                        </div>
                                                    </div>
                                                    <Divider />
                                                    <div style={{
                                                        paddingTop: '20px'
                                                    }}>
                                                        <FormInput
                                                            label="Name"
                                                            name="Name"
                                                            id="Name"
                                                            defaultValue={e.cardName}
                                                            required fluid
                                                            onChange={(e) => this.editName(e)}
                                                            showSuccessIndicator={false}
                                                        />

                                                        <div style={{ display: "flex", flexDirection: "column", marginTop: "15px" }}>
                                                            <Text> Change Icon</Text>
                                                            <Input type="file" id="upload" style={{ display: 'none' }} onChange={value => this.onFileChoose(value)}></Input>
                                                            <Button onClick={() => this.fileUpload()} className='cameraBtn'>
                                                                <PaperclipIcon styles={{ marginRight: "5px" }} /> Upload Photo
                                                        </Button>
                                                        </div>
                                                        {this.state.base64Image ? <img src={this.state.base64Image} className="badgePageEditIcon" /> :
                                                         <img src={e.cardImage} className="badgePageEditIcon" />}
                                                        <div className="outerDivToggleRadioGroup">
                                                            <Text styles={{ marginRight: "5px" }}> Active Status </Text>
                                                            <Toggle defaultChecked={e.isActive} icons={false} onChange={(e) => this.editActiveStatus(e)} ></Toggle>
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
                                    onCancel={()=>this.cancel()}
                                    trigger={<Button icon={<EditIcon />} text iconOnly>Edit</Button>}
                                /></div>
                            </td>
                        </tr>
                    })}


                </table>

            </div>
        );
    }
}



export default Badge;
