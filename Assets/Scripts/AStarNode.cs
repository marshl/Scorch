using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class AStarNode
{
    public HexTile hextile;

    /*public float fScore;
    public float gScore;
    public float hScore;*/

    public AStarNode parent;

    public float costToThisPoint;
    public float distToDestinationNode;

    public bool isClosed;

    public float GetFScore()
    {
        return this.costToThisPoint + this.distToDestinationNode;
    }

    //float m_fGScore; /// The amount of distance travelled to get to this point
    //float m_fHScore; /// The distance from this point to the destination node
    //float m_fFScore; /// The sum of the G and H score

    public AStarNode( HexTile _hexTile )
    {
        this.hextile = _hexTile;
    }

    public void Prepare( AStarNode _endNode )
    {
        this.isClosed = false;
        this.costToThisPoint = 0.0f;
        this.parent = null;
        this.distToDestinationNode = this.GetCostTo( _endNode );
    }

    public float GetCostTo( AStarNode _otherNode )
    {
        return (this.hextile.GetNormalisedPosition() - _otherNode.hextile.GetNormalisedPosition()).magnitude;
    }
}