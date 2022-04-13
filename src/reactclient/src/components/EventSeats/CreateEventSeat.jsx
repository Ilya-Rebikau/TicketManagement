import React, {Component} from "react";
import {NavLink} from 'react-router-dom';

export class CreateEventSeat extends Component{
    constructor(props){
        super(props);

        this.state={
            EventAreaId:"",
            Row:"",
            number:"",
            State:""
        };
    }

    onCreateClick(){

    }

    changeEventAreaId = (e) =>{
        this.setState({
            EventAreaId:e.target.value
        });
    }

    changeRow = (e) =>{
        this.setState({
            Row:e.target.value
        });
    }

    changeNumber = (e) =>{
        this.setState({
            Number:e.target.value
        });
    }

    changeState = (e) =>{
        this.setState({
            State:e.target.value
        });
    }

    render(){
        const {
            EventAreaId,
            Row,
            Number,
            State
        }=this.state;

        return(
            <div>
                <h1>Title</h1>
                <hr />
                <div className="row">
                    <div className="col-md-4">
                        <form method='POST'>
                            <div className="form-group">
                                <label className="control-label">EventAreaId</label>
                                <input type="number" className="form-control" value={EventAreaId} onChange={this.changeEventAreaId} />
                            </div>
                            <div className="form-group">
                                <label className="control-label">Row</label>
                                <input type="number" className="form-control" value={Row} onChange={this.changeRow} />
                            </div>
                            <div className="form-group">
                                <label className="control-label">Number</label>
                                <input type="number" className="form-control" value={Number} onChange={this.changeNumber} />
                            </div>
                            <div className="form-group">
                                <label className="control-label">State</label>
                                <input type="text" className="form-control" value={State} onChange={this.changeState} />
                            </div>
                            <div className="form-group">
                                <input type="submit" value="Create" className="btn btn-success" />
                            </div>
                        </form>
                    </div>
                </div>
                <div>
                    <NavLink className="btn btn-primary" to="/eventseats">Back</NavLink>
                </div>
            </div>
        )
    }
}