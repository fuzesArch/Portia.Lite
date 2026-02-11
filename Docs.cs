using System;

namespace Portia.Lite
{
    public static class Docs
    {
        public static string AddDropDownMark(
            this string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return text;
            }

            return "< " + text;
        }

        public static string AddDropDownNote(
            this string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return text;
            }

            return
                $"{text}{Environment.NewLine}Right-click to change the input structure!";
        }

        public static string TemporaryUnused => "TODO";

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
            "through customizable Task inputs.";

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

        public static string Constraint =>
            "Defines a localized graph process that evaluates " +
            "nodes or edges based on their geometric and topological behavior.";

        public static string IndexConstraint =>
            "Defines a localized graph process that evaluates both " +
            "Nodes AND Edges based on their numerical Index.";

        public static string TypeConstraint =>
            "Defines a localized graph process that evaluates both " +
            "Nodes AND Edges based on their text-based Type value.";

        public static string NodeAdjacentEdgeType =>
            "A topological query that validates a Node based on the Type " +
            "of its connected Edges. Useful for identifying hybrid structural junctions, or " +
            "setting the Type of a Node based on its neighbour.";

        public static string NodeAdjacentVectorSimilarity =>
            "A topological query that validates a Node based on the Vectors " +
            "of its connected Edges. Useful for identifying hybrid structural junctions, or " +
            "setting the Type of a Node based on its neighbouring Edge constellation.";

        public static string NodeInBoundary =>
            "A topological query that validates a Node based on its centroid's containment by the input boundary Breps." +
            "Useful to set initial Node Types or Indices as basic categorization.";

        public static string NodeAdjacency =>
            "A rule that evaluates a Node's direct neighbouring Edge count.";

        public static string NodeProximity =>
            "A rule that checks the spatial distance between Nodes " +
            "regardless of direct edge connections.";

        public static string NodeVectorSum =>
            "A rule that calculates the resultant force or orientation of a node " +
            "by summing the directional vectors of all connected edges.";

        public static string IsLeafNode =>
            "A binary validation rule that identifies 'leaf' nodes—elements " +
            "with only one connection—at the termination of a graph branch.";

        public static string EdgeSimilarity =>
            "A geometric rule that compares an Edge's start tangent vector to the input vectors by the input conditions.";

        public static string EdgeLength =>
            "A rule that evaluates the physical length of an Edge curve.";

        public static string StartAdjacency =>
            "A rule that evaluates the direct neighbouring Edge count of an Edge's start Node.";


        public static string EndAdjacency =>
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

        public static string SetNodeTypes =>
            "A modification Task that assigns specific Type string values to " +
            "existing graph Nodes for categorization.";

        public static string SetEdgeTypes =>
            "A modification Task that assigns specific Type string values to " +
            "existing graph Edges for categorization.";

        public static string GetNodes =>
            "A retrieval (query) Task that extracts Node identity values and geometry data " +
            "from the graph for downstream modeling.";

        public static string GetEdges =>
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

        public static string Selection =>
            "A criteria used to isolate specific Nodes or Edges " +
            "for targeted graph modification or retrieval.";

        public static string Selections =>
            "A set of Selection criteria used to isolate specific Nodes or Edges " +
            "for targeted graph modification or retrieval.";

        public static string Types =>
            "The specific text-based Type values to be assigned to the " +
            "elements that get captured by the current input Selection logics.";

        public static string Logics =>
            "The graph logic rules used to evaluate and verify the topological or " +
            "geometric integrity of the elements that get captured " +
            "by the current input Selection logics.";

        public static string Name =>
            "A unique identifier used to label logic results and " +
            "dynamically name the corresponding Portia output fields for easy tracking.";

        public static string DirectionLines =>
            "Edge directions (as a Lines) that will be checked against every Node's adjacent Edges for " +
            "a successful constellation match. Only one intersection point is allowed between the Lines' start points since a " +
            "constellation must radiate outwards.";

        public static string StrictMatch =>
            "When enabled, the node must have an exact count of connected edges " +
            "matching the number of defined NodeVectors to pass the rule.";

        public static string AngleTolerance =>
            "Allowed angle deviation between vectors IN DEGREES. Used during similarity comparison between " +
            "outgoing Node vectors (constellations).";

        public static string Bidirectional =>
            "When enabled, the rule validates the connection regardless of edge direction, " +
            "checking both start-to-end and end-to-start orientations.";

        public static string ByLogicSelection =>
            "A selection rule that identifies elements based on whether they satisfy " +
            "a specific topological or geometric Logic condition.";

        public static string ByWrapSelection =>
            "A spatial selection rule that captures Nodes whose centroids are " +
            "contained within the volume of the specified input Breps.";

        public static string ByIntersectionSelection =>
            "A spatial selection rule that identifies Edges based on their physical intersection " +
            "AND containment with(in) the volumes of specified Breps.";

        public static string CompositeConstraint =>
            "A sophisticated constraint rule that combines multiple constraints criteria " +
            "using a boolean Gate logic in order to capture graph elements.";

        public static string Boundary =>
            "Boundary Brep used to define the spatial volumes " +
            "for Node centroid containment checks.";

        public static string Breps =>
            "The geometric boundary volume used to evaluate intersections with " +
            "AND containment by Edges.";

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