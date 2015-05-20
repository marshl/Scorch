using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class AStarNode
{
    public HexTile hextile;
    public AStarNode parent;

    public float costToThisPoint;
    public float distToDestinationNode;

    public bool isClosed;

    public AStarNode( HexTile _hexTile )
    {
        this.hextile = _hexTile;
    }

    public float GetFScore()
    {
        return this.costToThisPoint + this.distToDestinationNode;
    }

    public void Initialise( AStarNode _endNode )
    {
        this.isClosed = false;
        this.costToThisPoint = 0.0f;
        this.parent = null;
        this.distToDestinationNode = this.GetCostTo( _endNode );
    }

    public float GetCostTo( AStarNode _otherNode )
    {
        return ( this.hextile.GetNormalisedPosition() - _otherNode.hextile.GetNormalisedPosition() ).magnitude;
    }
}