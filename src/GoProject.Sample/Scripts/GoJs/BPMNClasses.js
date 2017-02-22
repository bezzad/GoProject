"use strict";
/*
*  Copyright (C) 1998-2017 by Northwoods Software Corporation. All Rights Reserved.
*/

var diagram;

var myLocation = {  // this controls the data properties used by data binding conversions
    x: "sepalLength",
    y: "sepalWidth"
}

// swimlanes
var MINLENGTH = 400;  // this controls the minimum length of any swimlane
var MINBREADTH = 20;  // this controls the minimum breadth of any non-collapsed swimlane


// Contains PoolLink and BPMNLinkingTool classes for the BPMN sample

// PoolLink, a special Link class for message flows from edges of pools

function PoolLink() {
    go.Link.call(this);
}
go.Diagram.inherit(PoolLink, go.Link);

/** @override */
PoolLink.prototype.getLinkPoint = function (node, port, spot, from, ortho, othernode, otherport) {
    var r = new go.Rect(port.getDocumentPoint(go.Spot.TopLeft),
                        port.getDocumentPoint(go.Spot.BottomRight));
    var op = go.Link.prototype.getLinkPoint.call(this, othernode, otherport, spot, from, ortho, node, port);

    var below = op.y > r.centerY;
    var y = below ? r.bottom : r.top;
    if (node.category === "privateProcess") {
        if (op.x < r.left) return new go.Point(r.left, y);
        if (op.x > r.right) return new go.Point(r.right, y);
        return new go.Point(op.x, y);
    } else { // otherwise get the standard link point by calling the base class method
        return go.Link.prototype.getLinkPoint.call(this, node, port, spot, from, ortho, othernode, otherport);
    }
};

// If there are two links from & to same node... and pool is offset in X from node... the link toPoints collide on pool
/** @override */
PoolLink.prototype.computeOtherPoint = function (othernode, otherport) {
    var op = go.Link.prototype.computeOtherPoint(this, othernode, otherport);
    var node = this.toNode;
    if (node === othernode) node = this.fromNode;
    if (othernode.category === "privateProcess") {
        op.x = node.getDocumentPoint(go.Spot.MiddleBottom).x;
    } else {
        if ((node === this.fromNode) ^ (node.actualBounds.centerY < othernode.actualBounds.centerY)) {
            op.x -= 1;
        } else {
            op.x += 1;
        }
    }
    return op;
};

/** @override */
PoolLink.prototype.getLinkDirection = function (node, port, linkpoint, spot, from, ortho, othernode, otherport) {
    if (node.category === "privateProcess") {
        var p = port.getDocumentPoint(go.Spot.Center);
        var op = otherport.getDocumentPoint(go.Spot.Center);
        var below = op.y > p.y;
        return below ? 90 : 270;
    } else {
        return go.Link.prototype.getLinkDirection.call(this, node, port, linkpoint, spot, from, ortho, othernode, otherport);
    }
};


// BPMNLinkingTool, a custom linking tool to switch the class of the link created.

function BPMNLinkingTool() {
    go.LinkingTool.call(this);
    // don't allow user to create link starting on the To node
    this.direction = go.LinkingTool.ForwardsOnly;
    this.temporaryLink.routing = go.Link.Orthogonal;
    this.linkValidation = function (fromnode, fromport, tonode, toport) {
        return BPMNLinkingTool.validateSequenceLinkConnection(fromnode, fromport, tonode, toport) ||
               BPMNLinkingTool.validateMessageLinkConnection(fromnode, fromport, tonode, toport);
    };
}
go.Diagram.inherit(BPMNLinkingTool, go.LinkingTool);

/** @override */
BPMNLinkingTool.prototype.insertLink = function (fromnode, fromport, tonode, toport) {
    var lsave = null;
    // maybe temporarily change the link data that is copied to create the new link
    if (BPMNLinkingTool.validateMessageLinkConnection(fromnode, fromport, tonode, toport)) {
        lsave = this.archetypeLinkData;
        this.archetypeLinkData = { category: "msg" };
    }

    // create the link in the standard manner by calling the base method
    var newlink = go.LinkingTool.prototype.insertLink.call(this, fromnode, fromport, tonode, toport);

    // maybe make the label visible
    if (fromnode.category === "gateway") {
        var label = newlink.findObject("Label");
        if (label !== null) label.visible = true;
    }

    // maybe restore the original archetype link data
    if (lsave !== null) this.archetypeLinkData = lsave;
    return newlink;
};

// static utility validation routines for linking & relinking as well as insert link logic

// in BPMN, can't link sequence flows across subprocess or pool boundaries
BPMNLinkingTool.validateSequenceLinkConnection = function (fromnode, fromport, tonode, toport) {
    if (fromnode.category === null || tonode.category === null) return true;

    // if either node is in a subprocess, both nodes must be in same subprocess (not even Message Flows) 
    if ((fromnode.containingGroup !== null && fromnode.containingGroup.category === "subprocess") ||
        (tonode.containingGroup !== null && tonode.containingGroup.category === "subprocess")) {
        if (fromnode.containingGroup !== tonode.containingGroup) return false;
    }

    if (fromnode.containingGroup === tonode.containingGroup) return true;  // a valid Sequence Flow
    // also check for children in common pool
    var common = fromnode.findCommonContainingGroup(tonode);
    return common != null;
};

// in BPMN, Message Links must cross pool boundaries
BPMNLinkingTool.validateMessageLinkConnection = function (fromnode, fromport, tonode, toport) {
    if (fromnode.category === null || tonode.category === null) return true;

    if (fromnode.category === "privateProcess" || tonode.category === "privateProcess") return true;

    // if either node is in a subprocess, both nodes must be in same subprocess (not even Message Flows) 
    if ((fromnode.containingGroup !== null && fromnode.containingGroup.category === "subprocess") ||
        (tonode.containingGroup !== null && tonode.containingGroup.category === "subprocess")) {
        if (fromnode.containingGroup !== tonode.containingGroup) return false;
    }

    if (fromnode.containingGroup === tonode.containingGroup) return false;  // an invalid Message Flow
    // also check for children in common pool
    var common = fromnode.findCommonContainingGroup(tonode);
    return common === null;
};

//------------------------------------------  pools / lanes   ----------------------------------------------


// compute the minimum size of a Pool Group needed to hold all of the Lane Groups
function computeMinPoolSize(pool) {
    // assert(pool instanceof go.Group && pool.category === "Pool");
    var len = MINLENGTH;
    pool.memberParts.each(function (lane) {
        // pools ought to only contain lanes, not plain Nodes
        if (!(lane instanceof go.Group)) return;
        var holder = lane.placeholder;
        if (holder !== null) {
            var sz = holder.actualBounds;
            len = Math.max(len, sz.width);
        }
    });
    return new go.Size(len, NaN);
}

// compute the minimum size for a particular Lane Group
function computeLaneSize(lane) {
    // assert(lane instanceof go.Group && lane.category !== "Pool");
    var sz = computeMinLaneSize(lane);
    if (lane.isSubGraphExpanded) {
        var holder = lane.placeholder;
        if (holder !== null) {
            var hsz = holder.actualBounds;
            sz.height = Math.max(sz.height, hsz.height);
        }
    }
    // minimum breadth needs to be big enough to hold the header
    var hdr = lane.findObject("HEADER");
    if (hdr !== null) sz.height = Math.max(sz.height, hdr.actualBounds.height);
    return sz;
}

// determine the minimum size of a Lane Group, even if collapsed
function computeMinLaneSize(lane) {
    if (!lane.isSubGraphExpanded) return new go.Size(MINLENGTH, 1);
    return new go.Size(MINLENGTH, MINBREADTH);
}

// Add a lane to pool (lane parameter is lane above new lane)
function addLaneEvent(diagram, lane) {
    diagram.startTransaction("addLane");
    if (lane !== null && lane.data.category === "Lane") {
        // create a new lane data object
        var shape = lane.findObject("SHAPE");
        var size = new go.Size(shape.width, MINBREADTH);
        //size.height = MINBREADTH;
        var newlanedata = {
            category: "Lane",
            text: "بخش",
            color: "white",
            isGroup: true,
            loc: go.Point.stringify(new go.Point(lane.location.x, lane.location.y + 1)), // place below selection
            size: go.Size.stringify(size),
            group: lane.data.group
        };
        // and add it to the model
        diagram.model.addNodeData(newlanedata);
    }
    diagram.commitTransaction("addLane");
}



// define a custom ResizingTool to limit how far one can shrink a lane Group
function LaneResizingTool() {
    go.ResizingTool.call(this);
}
go.Diagram.inherit(LaneResizingTool, go.ResizingTool);

LaneResizingTool.prototype.isLengthening = function () {
    return (this.handle.alignment === go.Spot.Right);
};

/** @override */
LaneResizingTool.prototype.computeMinSize = function () {
    var lane = this.adornedObject.part;
    // assert(lane instanceof go.Group && lane.category !== "Pool");
    var sz = computeMinLaneSize(lane);  // get the absolute minimum size
    if (this.isLengthening()) {  // compute the minimum length of all lanes
        sz = computeMinPoolSize(lane.containingGroup);
        sz.width = Math.max(sz.width, sz.width);
    } else {  // find the minimum size of this single lane
        sz = computeLaneSize(lane);
        sz.width = Math.max(sz.width, sz.width);
        sz.height = Math.max(sz.height, sz.height);
    }
    return sz;
};

/** @override */
LaneResizingTool.prototype.canStart = function () {
    if (!go.ResizingTool.prototype.canStart.call(this)) return false;

    // if this is a resize handle for a "Lane", we can start.
    var diagram = this.diagram;
    if (diagram === null) return null;
    var handl = this.findToolHandleAt(diagram.firstInput.documentPoint, this.name);
    if (handl === null || handl.part === null || handl.part.adornedObject === null || handl.part.adornedObject.part === null) return false;
    return (handl.part.adornedObject.part.category === "Lane");
}

/** @override */
LaneResizingTool.prototype.resize = function (newr) {
    var lane = this.adornedObject.part;
    if (this.isLengthening()) {  // changing the length of all of the lanes
        lane.containingGroup.memberParts.each(function (lane) {
            if (!(lane instanceof go.Group)) return;
            var shape = lane.resizeObject;
            if (shape !== null) {  // set its desiredSize length, but leave each breadth alone
                shape.width = newr.width;
            }
        });
    } else {  // changing the breadth of a single lane
        go.ResizingTool.prototype.resize.call(this, newr);
    }
    relayoutDiagram();  // now that the lane has changed size, layout the pool again
};
// end LaneResizingTool class

// define a custom grid layout that makes sure the length of each lane is the same
// and that each lane is broad enough to hold its subgraph
function PoolLayout() {
    go.GridLayout.call(this);
    this.cellSize = new go.Size(1, 1);
    this.wrappingColumn = 1;
    this.wrappingWidth = Infinity;
    this.isRealtime = false;  // don't continuously layout while dragging
    this.alignment = go.GridLayout.Position;
    // This sorts based on the location of each Group.
    // This is useful when Groups can be moved up and down in order to change their order.
    this.comparer = function (a, b) {
        var ay = a.location.y;
        var by = b.location.y;
        if (isNaN(ay) || isNaN(by)) return 0;
        if (ay < by) return -1;
        if (ay > by) return 1;
        return 0;
    };
}
go.Diagram.inherit(PoolLayout, go.GridLayout);

/** @override */
PoolLayout.prototype.doLayout = function (coll) {
    var diagram = this.diagram;
    if (diagram === null) return;
    diagram.startTransaction("PoolLayout");
    var pool = this.group;
    if (pool !== null && pool.category === "Pool") {
        // make sure all of the Group Shapes are big enough
        var minsize = computeMinPoolSize(pool);
        pool.memberParts.each(function (lane) {
            if (!(lane instanceof go.Group)) return;
            if (lane.category !== "Pool") {
                var shape = lane.resizeObject;
                if (shape !== null) {  // change the desiredSize to be big enough in both directions
                    var sz = computeLaneSize(lane);
                    shape.width = (isNaN(shape.width) ? minsize.width : Math.max(shape.width, minsize.width));
                    shape.height = (!isNaN(shape.height)) ? Math.max(shape.height, sz.height) : sz.height;
                    var cell = lane.resizeCellSize;
                    if (!isNaN(shape.width) && !isNaN(cell.width) && cell.width > 0) shape.width = Math.ceil(shape.width / cell.width) * cell.width;
                    if (!isNaN(shape.height) && !isNaN(cell.height) && cell.height > 0) shape.height = Math.ceil(shape.height / cell.height) * cell.height;
                }
            }
        });
    }
    // now do all of the usual stuff, according to whatever properties have been set on this GridLayout
    go.GridLayout.prototype.doLayout.call(this, coll);
    diagram.commitTransaction("PoolLayout");
};
// end PoolLayout class


// Add a port to the specified side of the selected nodes.   name is beN  (be0, be1)
// evDim is 5 for Interrupting, 6 for non-Interrupting
function addActivityNodeBoundaryEvent(diagram, evType, evDim) {
    diagram.startTransaction("addBoundaryEvent");
    diagram.selection.each(function (node) {
        // skip any selected Links
        if (!(node instanceof go.Node)) return;
        if (node.data && (node.data.category === "activity" || node.data.category === "subprocess")) {
            // compute the next available index number for the side
            var i = 0;
            var defaultPort = node.findPort("");
            while (node.findPort("be" + i.toString()) !== defaultPort) i++;           // now this new port name is unique within the whole Node because of the side prefix
            var name = "be" + i.toString();
            if (!node.data.boundaryEventArray) { diagram.model.setDataProperty(node.data, "boundaryEventArray", []); }       // initialize the Array of port data if necessary
            // create a new port data object
            var newportdata = {
                portId: name,
                eventType: evType,
                eventDimension: evDim,
                color: "white",
                alignmentIndex: i
                // if you add port data properties here, you should copy them in copyPortData above  ** BUG...  we don't do that.
            };
            // and add it to the Array of port data
            diagram.model.insertArrayItem(node.data.boundaryEventArray, -1, newportdata);
        }
    });
    diagram.commitTransaction("addBoundaryEvent");
}

// this is called after nodes have been moved or lanes resized, to layout all of the Pool Groups again
function relayoutDiagram(diagram) {
    if (diagram === undefined || diagram === null) diagram = window.myDiagram;
    diagram.layout.invalidateLayout();
    diagram.findTopLevelGroups().each(function (g) { if (g.category === "Pool") g.layout.invalidateLayout(); });
    diagram.layoutDiagram();
}


// changes the item of the object
function rename(diagram, obj) {
    diagram.startTransaction("rename");
    var newName = prompt("Rename " + obj.part.data.item + " to:");
    diagram.model.setDataProperty(obj.part.data, "item", newName);
    diagram.commitTransaction("rename");
}

// shows/hides gridlines
// to be implemented onclick of a button
function updateGridOption(diagram) {
    diagram.startTransaction("grid");
    var grid = document.getElementById("grid");
    diagram.grid.visible = grid.checked;
    diagram.commitTransaction("grid");
}


// enables/disables snapping tools, to be implemented by buttons
function updateSnapOption(diagram) {
    // no transaction needed, because we are modifying tools for future use
    var snap = document.getElementById("snap");
    if (snap.checked) {
        diagram.toolManager.draggingTool.isGridSnapEnabled = true;
        diagram.toolManager.resizingTool.isGridSnapEnabled = true;
    } else {
        diagram.toolManager.draggingTool.isGridSnapEnabled = false;
        diagram.toolManager.resizingTool.isGridSnapEnabled = false;
    }
}

function setCurrentFileName(diagram, name) {
    var currentFile = document.getElementById("currentFile");
    if (diagram.isModified) {
        name += "*";
    }
    currentFile.textContent = name;
}

function newDocument(diagram) {
    // checks to see if all changes have been saved
    if (diagram.isModified) {
        var save = confirm("Would you like to save changes ?");
        if (save) {
            saveDocument();
        }
    }
    setCurrentFileName(diagram, "(Unsaved File)");
    // loads an empty diagram
    diagram.model = new go.GraphLinksModel();
    resetModel(diagram);
}

function resetModel(diagram) {
    diagram.model.undoManager.isEnabled = true;
    diagram.model.linkFromPortIdProperty = "fromPort";
    diagram.model.linkToPortIdProperty = "toPort";

    diagram.model.copiesArrays = true;
    diagram.model.copiesArrayObjects = true;
    diagram.isModified = false;
}

function saveDocument(diagram, apiName) {
    if (diagram.isModified) {
        saveDiagramProperties(diagram);
        var data = JSON.parse(myDiagram.model.toJson());
        if (diagram.model.id !== undefined && diagram.model.id !== null) {
            data.name = diagram.model.name;
            data.id = diagram.model.id;
        } else {
            data.name = prompt("Please enter diagram name", "Test Diagram1");
        }
        $.post(window.location.origin + '/' + apiName, data, function (d) {
            alert("Stored Successfull at path: " + d + "!");
        })
            .fail(function (d) { alert("Fail to store on path: " + d); });
        myDiagram.isModified = false; // save and have no changes
    }
}


// these functions are called when panel buttons are clicked

function loadJSON(diagram, file, callback) {
    jQuery.getJSON(file, function (jsondata) {
        // set these kinds of Diagram properties after initialization, not now
        diagram.addDiagramListener("InitialLayoutCompleted", function (e) { loadDiagramProperties(e, diagram); });  // defined below
        // create the model from the data in the JavaScript object parsed from JSON text
        //diagram.model = new go.GraphLinksModel(jsondata["nodes"], jsondata["links"]);
        diagram.model = go.Model.fromJson(jsondata);
        loadDiagramProperties(null, diagram);
        diagram.model.undoManager.isEnabled = true;
        diagram.isModified = false;
        diagram.isReadOnly = jsondata.isReadOnly;
        if (callback !== undefined)
            callback();
    });
}

// Store shared model state in the Model.modelData property
// (will be loaded by loadDiagramProperties)
function saveDiagramProperties(diagram) {
    diagram.model.modelData.position = go.Point.stringify(myDiagram.position);
}

// Called by loadFile and loadJSON.
function loadDiagramProperties(e, diagram) {
    // set Diagram.initialPosition, not Diagram.position, to handle initialization side-effects
    var pos = diagram.model.modelData.position;
    if (pos) diagram.initialPosition = go.Point.parse(pos);
}


function nodeInfo(d) {  // Tooltip info for a node data object
    var str = d.text + "\n\n";
    str += "Category: " + d.category + "\n";

    if (d.details !== undefined && d.details !== null) {
        str += "\n\n------ Detials ------\n";
        for (var prop in d.details) {
            str += prop + ":   " + d.details[prop] + "\n";
        }
    }
    return str;
}