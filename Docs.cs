using System;

namespace Portia.Lite
{
    public static class Docs
    {
        public static string NumericRelation =>
            "Numerical comparison logic (Equal, GreaterThan, ..) for filtering floating point numbers.";

        public static string StringRelation =>
            "Text-based filter (Contains, StartsWith, ..) used mostly for node / edge Type processes.";

        public static string BooleanCondition =>
            "Logical Boolean Toggle: If True, the rule captures elements that satisfy the condition. " +
            "If False, the rule is inverted to capture elements that do not.";

        public static string MatchAll =>
            "Similarly to the Gate component, this Boolean operator only returns with True, if the given condition(s) is true " +
            "for ALL elements that are evaluated. If its False, it is enough that ANY of the evaluated elements match the condition(s)";

        public static string Gate =>
            "Boolean operator (AND, OR) to combine multiple conditions into one filter. " +
            "AND means that all conditions must be True, OR means that any of the conditions is enough to be True.";

        public static string NumericValue =>
            "The target numerical (double a.k.a. floating point number) value for mathematical filtering.";

        public static string StringValue =>
            "The target string value used for text-based filtering.";

        public static string PortiaComponent =>
            "The core building block of the Portia ecosystem, " +
            "designed to manage complex graph logic with architectural data " +
            "through customizable Task inputs." + Environment.NewLine +
            Environment.NewLine +
            "The Portia module ecosystem is developed and maintained by Bálint Füzes." +
            Environment.NewLine +
            "For development inquiries, enterprise customization requests, or technical feedback, please reach out via the following channels:" +
            Environment.NewLine + "https://fuzesarch.com/" +
            Environment.NewLine + "balint@fuzesarch.com";

        public static string Condition =>
            "Defines a logical filter rule based on the relationship " +
            "with a selection relation type and a specific target value.";

        public static string DoubleCondition =>
            "Defines a logical filter rule based on the relationship " +
            "with a double-based selection relation type and a specific double target value.";

        public static string VectorCondition =>
            "Defines a logical filter rule based on the geometric relationship " +
            "between two vectors.";

        public static string StringCondition =>
            "Defines a logical filter rule based on the relationship " +
            "with a text-based relation type and a specific string target value.";

        public static string BoundaryCondition =>
            "Defines a logical filter based on the geometric relationship " +
            "between points and boundaries, where a boundary is a closed Brep evaluated for point containment.";

        public static string Identity =>
            "Defines the primary identification rule for graph elements " +
            "using either a numerical Index or a text-based Type.";

        public static string Index =>
            "The specific numerical index used to identify and map " +
            "a unique element (node or edge) within the graph structure.";

        public static string Type =>
            "A user-defined text string used to categorize and map " +
            "specific elements (nodes or edges) within the graph structure.";

        public static string Rule =>
            "Defines a localized graph process that evaluates " +
            "nodes or edges based on their geometric and topological behavior.";

        public static string NodeRule =>
            "Defines a localized graph process that evaluates " +
            "Nodes based on their geometric and topological behavior.";

        public static string EdgeRule =>
            "Defines a localized graph process that evaluates " +
            "Edges based on their geometric and topological behavior.";

        public static string IndexRule =>
            "Defines a localized graph process that evaluates both " +
            "Nodes AND Edges based on their numerical Index.";

        public static string TypeRule =>
            "Defines a localized graph process that evaluates both " +
            "Nodes AND Edges based on their text-based Type value.";

        public static string NodeAdjacentEdgeType =>
            "A topological query that validates a Node based on the Type " +
            "of its connected Edges. Useful for identifying hybrid structural junctions, or " +
            "setting the Type of a Node based on its neighbour.";

        public static string NodeAdjacentEdgeVectorSimilarity =>
            "A topological query that validates a Node based on the outgoing vectors " +
            "of its connected Edges. Useful for identifying hybrid structural junctions, or " +
            "setting the Type of a Node based on its neighbouring Edge constellation.";

        public static string NodeInBoundary =>
            "A topological query that validates a Node based on its centroid's " +
            "containment by the input boundary Breps. Useful to set initial Node Types " +
            "or Indices for basic categorization.";

        public static string EdgeInBoundary =>
            "A topological query that validates an Edge based on its start or end point's " +
            "containment by the input boundary Breps. Useful to set initial Edge Types " +
            "or Indices for basic categorization.";

        public static string NodeAdjacency =>
            "A rule that evaluates a Node's direct neighbouring Edge count.";

        public static string NodeProximity =>
            "A rule that checks the spatial distance between Nodes " +
            "regardless of direct edge connections.";

        public static string NodeVectorScalarSum =>
            "A rule that calculates the resultant force or orientation of a node " +
            "by summing the directional vectors of all connected edges." +
            Environment.NewLine +
            "0.0 / Perfect Symmetry: A cross(X) or a star where every force is perfectly cancelled out by an opposite one." +
            Environment.NewLine +
            "Close to 0.0 /	Balanced Junction: A standard 'T1 junction or a straight line (180°). The vectors point in opposite directions, 'pulling' the node equally." +
            Environment.NewLine +
            "1.0 /	Unbalanced / Corner: A 90° corner with two edges.The vectors don't cancel out; they combine to point toward the 'outside' of the corner." +
            Environment.NewLine +
            "High ( > 2.0) / Acute Bunching: Many edges all pointing in roughly the same direction.The node is 'heavily weighted' toward one side.";

        public static string IsLeafNode =>
            "A binary validation rule that identifies 'leaf' nodes—elements " +
            "with only one connection—at the termination of a graph branch.";

        public static string EdgeSimilarity =>
            "A geometric rule that compares an Edge's start tangent vector to the input vectors by the input conditions.";

        public static string EdgeCurveLength =>
            "A rule that evaluates the physical length of an Edge' curve.";

        public static string EdgeStartEndDistance =>
            "A rule that evaluates the virtual length between the Start and End Node Centroids of an Edge.";

        public static string StartDegree =>
            "A rule that evaluates the direct neighbouring Edge count of an Edge's start Node.";

        public static string EndDegree =>
            "A rule that evaluates the direct neighbouring Edge count of an Edge's end Node.";

        public static string EdgeStartIndex =>
            "A rule that evaluates / captures an Edge based on the numerical Index " +
            "of its start Node.";

        public static string EdgeStartType =>
            "A rule that evaluates / captures an Edge based on the text-based Type " +
            "of its end Node.";

        public static string EdgeEndIndex =>
            "A rule that evaluates / captures an Edge based on the numerical Index " +
            "of its end Node.";

        public static string EdgeEndType =>
            "A rule that evaluates / captures an Edge based on the text-based Type " +
            "of its end Node.";

        public static string IsLinearRule =>
            "A binary validation rule that determines if an Edge is perfectly straight " +
            "within document tolerance, distinguishing between linear members and curved geometry.";

        public static string Task =>
            "A specialized graph operation that executes " +
            "data modifications, queries or validation protocols by the Portia engine.";

        public static string SetCurves =>
            "A primary Task that translates geometric curves into graph-compatible " +
            "edge data for network processing.";

        public static string SetNodeIndices =>
            "A modification Task that assigns specific Index integer values to " +
            "existing graph Nodes for categorization.";

        public static string SetEdgeIndices =>
            "A modification Task that assigns specificIndex integer values to " +
            "existing graph Edges for categorization.";

        public static string SetNodeTypes =>
            "A modification Task that assigns specific Type string values to " +
            "existing graph Nodes for categorization.";

        public static string SetEdgeTypes =>
            "A modification Task that assigns specific Type string values to " +
            "existing graph Edges for categorization.";

        public static string FilterNodes =>
            "A retrieval (query) Task that extracts Node identity values and geometry data " +
            "from the graph for downstream modeling.";

        public static string FilterEdges =>
            "A retrieval (query) Task that extracts Edge identity values and geometry data " +
            "from the graph for downstream modeling.";

        public static string VerifyNodes =>
            "A validation Task that checks Nodes against selected logic rules " +
            "to ensure structural or topological integrity.";

        public static string VerifyEdges =>
            "A validation Task that checks Edges against selected logic rules " +
            "to ensure structural or topological integrity.";

        public static string Curves =>
            "The input geometric curves used to generate the " +
            "base network topology for the Portia engine.";

        public static string InitialEdgeTypes =>
            "The optional Type strings assigned to the Edges during the " +
            "initial graph generation process, used for categorization.";

        public static string Indices =>
            "The specific integer-based Index values to be assigned to the " +
            "elements that get captured by the current input Rule.";

        public static string Types =>
            "The specific text-based Type values to be assigned to the " +
            "elements that get captured by the current input Rule.";

        public static string NodeLogics =>
            "The graph logic rules that verify the topological or " +
            "geometric integrity of the Nodes that are captured " +
            "by the input Node rules!";

        public static string EdgeLogics =>
            "The graph logic rules that verify the topological or " +
            "geometric integrity of the Edges that are captured " +
            "by the input Edge rules!";

        public static string Name =>
            "A unique identifier used to label logic results and " +
            "dynamically name the corresponding Portia output fields for easy tracking.";

        public static string AngleTolerance =>
            "Allowed angle deviation between vectors IN DEGREES. Used during similarity comparison between " +
            "outgoing Node vectors (constellations).";

        public static string Bidirectional =>
            "When enabled, the rule validates the connection regardless of edge direction, " +
            "checking both start-to-end and end-to-start orientations.";

        public static string CompositeRule =>
            "A sophisticated rule rule that combines multiple rules " +
            "using a boolean Gate logic in order to capture graph Nodes or Edges.";

        public static string Boundary =>
            "Boundary Brep used to define the spatial volumes " +
            "for Node centroid containment checks.";

        public static string StrictlyIn =>
            "A boolean toggle that determines if elements lying exactly on the " +
            "Brep boundary surface are included in the selection or not.";

        public static string DeconstructItem =>
            "A helper component used to unlock the details of a graph element (both Node or Edge), " +
            "extracting its identity, geometric content (Centroid or Curve), and its raw JSON format data. " +
            "Connect to Portia Nodes and Edges outputs!";

        public static string GraphItem =>
            "The proprietary 'Sovereign Goo' wrapper that encapsulates a graph element (Node or Edge), " +
            "carrying its full topological intelligence and metadata through the network.";

        public static string Json =>
            "The universal, transaction-ready string representation of a graph element (Node or Edge), " +
            "allowing for seamless data exchange between Portia and external logic engines.";

        public static string Geometries =>
            "The projected geometric representation of an element: a Node's Centroid or Edge's Curve.";

        public static string Vector => "3D vector.";
    }
}