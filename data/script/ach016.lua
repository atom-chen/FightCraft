
x100016_g_scriptId = 100016
x100016_g_stageId = 2

--�¼�����
function x100016_OnEventFilter(param1, param2, param3, param4, param5)

	if param4 ~= nil and tonumber(param4) == x100016_g_stageId then
  		return 1
  	else
  		return 0
  	end
  	
end

--�ɾ���ɽ���
function x100016_OnAchievementProgress(uuid, reachNum)
  	return 1
end

