
x100015_g_scriptId = 100015
x100015_g_stageId = 2

--�¼�����
function x100015_OnEventFilter(param1, param2, param3, param4, param5)

	if param4 ~= nil and tonumber(param4) == x100015_g_stageId then
  		return 1
  	else
  		return 0
  	end
  	
end

--�ɾ���ɽ���
function x100015_OnAchievementProgress(uuid, reachNum)
  	return 1
end

